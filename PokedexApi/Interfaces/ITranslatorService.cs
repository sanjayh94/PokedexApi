using PokedexApi.Models;

namespace PokedexApi.Interfaces
{
    public interface ITranslatorService
    {
        PokemonTranslated GetTranslatedPokemonDescriptionWithConditions(string pokemonName);
    }
}
