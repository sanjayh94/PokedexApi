using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokedexApi.Interfaces;
using PokedexApi.Models;
using System;
using System.Threading.Tasks;

namespace PokedexApi.Controllers
{
    /// <summary>
    /// Controller class to handle '/pokemon' route calls
    /// </summary>
    [Route("pokemon")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        #region DependencyInjectionVariables
        // Setting up private Dependency Injection Variable
        private readonly ILogger<PokemonController> _logger;
        private readonly IPokemonService _pokemonService;
        private readonly ITranslatorService _translatorService;
        #endregion

        public PokemonController(IPokemonService pokemonService, ITranslatorService translatorService, ILogger<PokemonController> logger)
        {
            //  Setting up the pokemonService and translationService using Dependency Injection.
            //  These services will call the third-party PokeApi API and retrieve Pokemon Info for the controller
            _pokemonService = pokemonService;
            _translatorService = translatorService;
            _logger = logger; // Setting up the logging service using Dependency Injection
        }

        /// <summary>
        /// Returns Pokemon Information for a given Pokemon name
        /// GET /pokemon/{pokemonName} route controller. The requests are first intercepted here after going through the Middleware.
        /// </summary>
        /// <param name="pokemonName">Name of pokemon you want the info for</param>
        /// <returns>ActionResult of PokemonDTO class</returns>
        [HttpGet("{pokemonName}")]
        public async Task<ActionResult<PokemonDTO>> GetPokemon(string pokemonName)
        {
            try
            {
                // Get Pokemon info using the pokemonService. Gets Pokemon Model in return.
                Pokemon pokemon = await _pokemonService.GetPokemon(pokemonName);

                // Send 404 NotFound response to user if Pokemon does not exist
                if (pokemon.ApiResponseStatus == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }

                // Return 200 response to User along with the PokemonDTO data.
                // The PokemonDTO class is a Data transfer Object that only sends a subset of the properties of the Pokemon class
                // It is best practice to use a DTO class as we wouldn't want to send unnecessary data to the user
                else if (pokemon.ApiResponseStatus == System.Net.HttpStatusCode.OK)
                {
                    var pokemonDTO = Utils.Utils.PokemonToDTO(pokemon);
                    return Ok(pokemonDTO);
                }
            }
            // Send 500 response if exception is thrown
            // Catches broad and generic exceptions at the moment. Ideally would split catch blocks to catch specific Exceptions
            catch (Exception ex)
            {
                _logger.LogError($"[GetPokemon Controller] Exception occured in Controller: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Assume failure if the code reaches here
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        ///  Return translated Pokemon description along with other basic Pokemon information for a given Pokemon Name
        ///  GET /pokemon/translated/{pokemonName} route controller. The requests are first intercepted here after going through the Middleware.
        /// </summary>
        /// <param name="pokemonName">Name of pokemon you want the info for</param>
        /// <returns>ActionResult of PokemonDTO class</returns>
        [HttpGet("translated/{pokemonName}")]
        public async Task<ActionResult<PokemonDTO>> GetTranslatedPokemonDescription(string pokemonName)
        {
            try
            {
                // Get PokemonTranslated model with Translated description using translatorService that calls an external API
                // This method returns translated descriptions with conditions
                // Conditions apply such that, If the Pokemon's habitat is `cave` or it's a legendary Pokemon then Yoda translation will be applied.
                // The rest of the Pokemons will have the Shakespeare translation.
                // The standard description is returned on fallback (such as if the translation API is rate-limited)

                PokemonTranslated pokemonTranslated = _translatorService.GetTranslatedPokemonDescriptionWithConditions(pokemonName);

                // Return a 404 NotFound response to user if the specified pokemon is not found
                if (pokemonTranslated.ApiResponseStatus == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }

                // Return 200 response to User along with the PokemonDTO data.
                // The PokemonDTO class is a Data transfer Object that only sends a subset of the properties of the PokemonTranslated class
                // It is best practice to use a DTO class as we wouldn't want to send unnecessary data to the user
                else if (pokemonTranslated.ApiResponseStatus == System.Net.HttpStatusCode.OK)
                {
                    PokemonDTO pokemonDTO = new PokemonDTO()
                    {
                        Name = pokemonTranslated.Name,
                        Habitat = pokemonTranslated.Habitat,
                        IsLegendary = pokemonTranslated.IsLegendary,
                        Description = pokemonTranslated.Description
                    };
                    return Ok(pokemonDTO);
                }
            }
            // Send 500 response if exception is thrown
            // Catches broad and generic exceptions at the moment. Ideally would split catch blocks to catch specific Exceptions
            catch (Exception ex)
            {
                _logger.LogError($"[GetTranslatedPokemonDescription Controller] Exception occured in Controller: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Assume failure if the code reaches here
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
