using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("{pokemonName}")]
        public async Task<ActionResult<PokemonDTO>> GetPokemon(string pokemonName)
        {

            return Ok("ok");
        }

        [HttpGet("translated/{pokemonName}")]
        public async Task<ActionResult<PokemonDTO>> GetTranslatedPokemonDescription(string pokemonName)
        {

            return Ok("ok");
        }
    }
}
