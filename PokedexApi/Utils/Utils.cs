using PokedexApi.Models;

namespace PokedexApi.Utils
{
    public class Utils
    {
        /// <summary>
        /// Static Util Class to convert an object of type Pokemon to PokemonDTO class. 
        /// This class is normally used by the controller to Select a subset of properties into a DTO (Data Transfer Object) as we would only expose a subset of the original Model.
        /// It is considered Best practice to use a DTO object for sending response payloads
        /// </summary>
        /// <param name="pokemon">Object of type Pokemon</param>
        /// <returns>PokemonDTO object with relevant properties from supplied Pokemon object</returns>
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
