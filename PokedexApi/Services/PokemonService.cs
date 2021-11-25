using Microsoft.Extensions.Logging;
using PokedexApi.Interfaces;
using PokedexApi.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokedexApi.Services
{
    /// <summary>
    /// Class which provides Services to Fetch Pokemon info. Implements IPokemon interface
    /// </summary>
    public class PokemonService : IPokemonService
    {
        #region DependencyInjectionPrivateVariables
        private readonly ILogger<PokemonService> _logger;
        private HttpClient _httpClient; 
        #endregion

        public PokemonService(HttpClient httpClient, ILogger<PokemonService> logger)
        {
            _logger = logger; // Set up logger

            // Using Dependency Injection to initialise the HttpClient for API calls
            _httpClient = httpClient;            
            _httpClient.BaseAddress = new Uri("https://pokeapi.co");
        }

        /// <summary>
        /// Async method to fetch pokemon info from third party API
        /// </summary>
        /// <param name="pokemonName">Pokemon name you would like info for</param>
        /// <returns>Task object of Pokemon Class</returns>
        public async Task<Pokemon> GetPokemon(string pokemonName)
        {
            // Currently configured to handle HTTP call retries with exponential backoff. (See Startup.cs)
            // Libraries such as Polly provide resilience and API transient-fault handling capabilities. 

            try
            {
                // Fetch Pokemon info using the BaseAddress defined in constructor and route url below
                string url = $"/api/v2/pokemon/{pokemonName}";
                var response = await _httpClient.GetAsync(url);
                _logger.LogInformation($"[GetPokemon] HTTP Response: {response.StatusCode}");
                _logger.LogInformation($"[GetPokemon] HTTP Reason Phrase: {response.ReasonPhrase}");

                // Add NotFound Status code to Pokemon object if APi responds with 404 so the controller can appropriately respond.
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"[GetPokemon] {pokemonName} not found");
                    return new Pokemon() { ApiResponseStatus = System.Net.HttpStatusCode.NotFound };
                }
                // Add InternalServerError Status code to Pokemon object if APi responds with Non success code so the controller can appropriately respond.
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"[GetPokemon] Response status: {response.StatusCode}");
                    _logger.LogError($"[GetPokemon] Response Reason phrase: {response.ReasonPhrase}");

                    return new Pokemon() { ApiResponseStatus = System.Net.HttpStatusCode.InternalServerError }; 
                }

                // Deserialize response json string to the .Net Pokemon model class
                string content = await response.Content.ReadAsStringAsync();
                Pokemon pokemon = Pokemon.FromJson(content);

                // Retrieve Pokemon species data using the link provided
                PokemonSpecies pokemonSpecies = GetPokemonSpecies(pokemon.Species.Url).Result;

                // Throw an Internal Server error if species can't be retrieved for some reason, So the controller can act upon it
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

        /// <summary>
        /// Async method to retrieve PokemonSpecies data using provided species link 
        /// </summary>
        /// <param name="pokemonSpeciesUrl">link for the pokemon species</param>
        /// <returns>Task object of PokemonSpecies class</returns>
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
