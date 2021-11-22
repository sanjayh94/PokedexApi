using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokedexApi.Interfaces;
using PokedexApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexApi.Controllers
{
    [Route("pokemon")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        #region DependencyInjectionVariables
        private readonly ILogger<PokemonController> _logger;
        private readonly IPokemonService _pokemonService;
        #endregion

        public PokemonController(IPokemonService pokemonService, ILogger<PokemonController> logger)
        {
            //  Setting up the pokemonService using Dependency Injection. This service will call the third-party PokeApi API and retrieve Pokemon Info for the controller
            _pokemonService = pokemonService; 
            _logger = logger; // Setting up the logging service using Dependency Injection
        }

        
        [HttpGet("{pokemonName}")]
        public async Task<ActionResult<PokemonDTO>> GetPokemon(string pokemonName)
        {
            var pokemon = await _pokemonService.GetPokemon(pokemonName);

            if (pokemon.ApiResponseStatus == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (pokemon.ApiResponseStatus == System.Net.HttpStatusCode.OK)
            {
                var pokemonDto = Utils.Utils.PokemonToDTO(pokemon);
                return Ok(pokemonDto);
            }

            return Problem(statusCode:500);         
        }

        [HttpGet("translated/{pokemonName}")]
        public async Task<ActionResult<PokemonDTO>> GetTranslatedPokemonDescription(string pokemonName)
        {

            return Ok("ok");
        }
    }
}
