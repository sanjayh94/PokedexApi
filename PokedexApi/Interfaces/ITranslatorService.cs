using PokedexApi.Models;

namespace PokedexApi.Interfaces
{
    /// <summary>
    /// Interface for fetching Pokemon Info with translated descriptions
    /// </summary>
    public interface ITranslatorService
    {
        PokemonTranslated GetTranslatedPokemonDescriptionWithConditions(string pokemonName);
    }
}
