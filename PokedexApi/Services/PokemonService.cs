using Microsoft.Extensions.Logging;
using PokedexApi.Interfaces;
using PokedexApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokedexApi.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly ILogger<PokemonService> _logger;
        private HttpClient _httpClient;

        public PokemonService(HttpClient httpClient, ILogger<PokemonService> logger)
        {
            _logger = logger;

            // Using Dependency Injection to initialise the HttpClient for API calls
            _httpClient = httpClient;            
            _httpClient.BaseAddress = new Uri("https://pokeapi.co");
        }

        public async Task<Pokemon> GetPokemon(string pokemonName)
        {
            // For Production, it would be useful to implement HTTP call retries with exponential backoff.
            // Libraries such as Polly provide resilience and API transient-fault handling capabilities. 

            try
            {
                string url = $"/api/v2/pokemon/{pokemonName}";
                var response = await _httpClient.GetAsync(url);
                _logger.LogInformation($"[GetPokemon] HTTP Response: {response.StatusCode}");
                _logger.LogInformation($"[GetPokemon] HTTP Reason Phrase: {response.ReasonPhrase}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"[GetPokemon] {pokemonName} not found");
                    return new Pokemon() { ApiResponseStatus = System.Net.HttpStatusCode.NotFound };
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"[GetPokemon] Response status: {response.StatusCode}");
                    _logger.LogError($"[GetPokemon] Response Reason phrase: {response.ReasonPhrase}");

                    return new Pokemon() { ApiResponseStatus = System.Net.HttpStatusCode.InternalServerError }; 
                }

                string content = await response.Content.ReadAsStringAsync();
                Pokemon pokemon = Pokemon.FromJson(content);

                // Retrieve Pokemon species using the link provided
                PokemonSpecies pokemonSpecies = GetPokemonSpecies(pokemon.Species.Url).Result;

                // Throw an Internal Server error if species can't be retrieved for some reason, Since it will be important for next steps :)
                if (pokemonSpecies == null)
                {
                    _logger.LogError($"[GetPokemon] Unable to retrieve Pokemon Species");
                    return new Pokemon() { ApiResponseStatus = System.Net.HttpStatusCode.InternalServerError };
                }

                pokemon.Species = pokemonSpecies;
                pokemon.ApiResponseStatus = System.Net.HttpStatusCode.OK;
                return pokemon;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetPokemon] There was a problem retrieving Pokemon {ex.Message}");
                return new Pokemon() { ApiResponseStatus = System.Net.HttpStatusCode.InternalServerError};
            }
        }

        private async Task<PokemonSpecies> GetPokemonSpecies(Uri pokemonSpeciesUrl)
        {
            try
            {                
                var response = await _httpClient.GetAsync(pokemonSpeciesUrl.AbsoluteUri);
                _logger.LogInformation($"[GetPokemonSpecies] HTTP Response: {response.StatusCode}");
                _logger.LogInformation($"[GetPokemonSpecies] HTTP Reason Phrase: {response.ReasonPhrase}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"[GetPokemonSpecies] {pokemonSpeciesUrl} not found");
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"[GetPokemonSpecies] Response status: {response.StatusCode}");
                    _logger.LogError($"[GetPokemonSpecies] Response Reason phrase: {response.ReasonPhrase}");

                    return null;
                }

                string content = await response.Content.ReadAsStringAsync();
                PokemonSpecies pokemonSpecies = PokemonSpecies.FromJson(content);
                return pokemonSpecies;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetPokemonSpecies] There was a problem retrieving Pokemon Species {ex.Message}");
                return null;
            }

        }
    }
}
