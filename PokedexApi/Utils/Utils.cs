using PokedexApi.Models;

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
                // For Simplicity, finds the first english description in the list of flavour Entries and replacing carriage returns and line feeds
                // There may be more sophisticated ways of doing this, but keeping it simple in this instance.
                Description = pokemon.Species.FlavorTextEntries.Find(x => x.Language.Name.Contains("en")).FlavorText.Replace("\n"," ").Replace("\r", " ").Replace("\f", " ")
            };
        }
    }
}
