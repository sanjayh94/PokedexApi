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
    public class TranslatorService : ITranslatorService
    {
        private readonly ILogger<TranslatorService> _logger;
        private readonly IPokemonService _pokemonService;
        private HttpClient _httpClient;

        public TranslatorService(IPokemonService pokemonService, HttpClient httpClient, ILogger<TranslatorService> logger)
        {
            _pokemonService = pokemonService;
            _logger = logger;

            // Using Dependency Injection to initialise the HttpClient for API calls
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://pokeapi.co");
        }

        public PokemonTranslated GetTranslatedPokemonDescriptionWithConditions(string pokemonName)
        {
            Pokemon pokemon = _pokemonService.GetPokemon(pokemonName).Result;

            if (pokemon.ApiResponseStatus == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"[GetTranslatedPokemonDescription] {pokemonName} not found");
                return new PokemonTranslated() { ApiResponseStatus = System.Net.HttpStatusCode.NotFound };
            }

            if (pokemon.ApiResponseStatus == System.Net.HttpStatusCode.OK)
            {
                PokemonTranslated pokemonTranslated = new PokemonTranslated() {
                    Name = pokemon.Name,
                    Habitat = pokemon.Species.Habitat.Name,
                    IsLegendary = pokemon.Species.IsLegendary,
                    // For Simplicity, finds the first english description in the list of flavour Entries and replacing carriage returns and line feeds
                    // There may be more sophisticated ways of doing this, but keeping it simple in this instance.
                    Description = pokemon.Species.FlavorTextEntries.Find(x => x.Language.Name.Contains("en")).FlavorText.Replace("\n", " ").Replace("\r", " ").Replace("\f", " ")
                };



            }

           return new PokemonTranslated() { ApiResponseStatus = System.Net.HttpStatusCode.InternalServerError };
        }
    }
}
