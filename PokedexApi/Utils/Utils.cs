using PokedexApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexApi.Utils
{
    public class Utils
    {
        public static PokemonDTO PokemonToDTO(Pokemon pokemon)
        {
            return new PokemonDTO() { Name = pokemon.Name };
        }
    }
}
