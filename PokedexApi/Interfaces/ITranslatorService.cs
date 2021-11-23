using PokedexApi.Models;

namespace PokedexApi.Interfaces
{
    public interface ITranslatorService
    {
        PokemonDTO GetTranslatedPokemonDescriptionWithConditions(string pokemonName);
    }
}
