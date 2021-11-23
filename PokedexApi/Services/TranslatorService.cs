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
        #region PrivateVariables
        private readonly ILogger<TranslatorService> _logger;
        private readonly IPokemonService _pokemonService;
        private HttpClient _httpClient;

        private enum TranslationEndpoint
        {
            Shakespeare,
            Yoda
        }
        #endregion

        public TranslatorService(IPokemonService pokemonService, HttpClient httpClient, ILogger<TranslatorService> logger)
        {
            _pokemonService = pokemonService;
            _logger = logger;

            // Using Dependency Injection to initialise the HttpClient for API calls
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.funtranslations.com");
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

                // Translate the pokemon description to the Yoda variant if Pokemon's habitat is cave or it's a legendary Pokemon. Otherwise, apply Shakespeare translation
                TranslationEndpoint translationEndpoint;
                if (pokemonTranslated.Habitat.ToLower() == "cave" || pokemonTranslated.IsLegendary == true)
                {
                    translationEndpoint = TranslationEndpoint.Yoda;
                }
                else
                {
                    translationEndpoint = TranslationEndpoint.Shakespeare;
                }

                string translatedDescription = GetTranslationAsync(text:pokemonTranslated.Description, translationEndpoint).Result;

                // If for some reason the description cannot be translated (API rate-limiting?), then return standard description
                if (translatedDescription == null)
                {
                    _logger.LogError("[GetTranslatedPokemonDescription] Unable to translate Pokemon's description. Returning standard description");
                    //pokemonTranslated.ApiResponseStatus = System.Net.HttpStatusCode.BadGateway;
                    pokemonTranslated.ApiResponseStatus = System.Net.HttpStatusCode.OK;

                    return pokemonTranslated;
                }
                else
                {
                    pokemonTranslated.Description = translatedDescription;
                    pokemonTranslated.ApiResponseStatus = System.Net.HttpStatusCode.OK;

                    return pokemonTranslated;
                }
            }

           return new PokemonTranslated() { ApiResponseStatus = System.Net.HttpStatusCode.InternalServerError };
        }

        private async Task<string> GetTranslationAsync(string text, TranslationEndpoint translationEndpoint) 
        {
            try
            { 
                string route = translationEndpoint.ToString().ToLower();
                string url= $"/translate/{route}?text={text}";
                var response = await _httpClient.GetAsync(url);
                _logger.LogInformation($"[GetTranslation] HTTP Response: {response.StatusCode}");
                _logger.LogInformation($"[GetTranslation] HTTP Reason Phrase: {response.ReasonPhrase}");

                TranslationResponse translation = TranslationResponse.FromJson(response.Content.ReadAsStringAsync().Result);

                if (response.IsSuccessStatusCode)
                {
                    return translation.Contents.Translated;
                }
                else
                {
                    _logger.LogError($"[GetTranslation] There was a problem Translating text: Code: {translation.Error.Code} Message: {translation.Error.Message}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetTranslation] There was a problem Translating text {ex.Message}");
                return null;
            }           
        }
    }
}
