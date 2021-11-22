using PokedexApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexApi.Interfaces
{
    public interface IPokemonService
    {
        Pokemon GetPokemon(string pokemonName); 
    }
}
