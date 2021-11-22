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
            return new PokemonDTO() {
                Name = pokemon.Name,
                Habitat = pokemon.Species.Habitat.Name,
                IsLegendary = pokemon.Species.IsLegendary,
                // For Simplicity, finds the first english description in the list of flavour Entries
                Description = pokemon.Species.FlavorTextEntries.Find(x => x.Language.Name.Contains("en")).FlavorText.Replace("\n"," ").Replace("\r", " ").Replace("\f", " ")
            };
        }
    }
}
