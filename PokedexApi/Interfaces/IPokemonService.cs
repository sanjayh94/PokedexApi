using PokedexApi.Models;
using System.Threading.Tasks;

namespace PokedexApi.Interfaces
{
    /// <summary>
    /// Interface for fetching Pokemon Info
    /// </summary>
    public interface IPokemonService
    {
        Task<Pokemon> GetPokemon(string pokemonName); 
    }
}
