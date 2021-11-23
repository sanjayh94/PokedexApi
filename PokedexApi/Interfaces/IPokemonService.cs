using PokedexApi.Models;
using System.Threading.Tasks;

namespace PokedexApi.Interfaces
{
    public interface IPokemonService
    {
        Task<Pokemon> GetPokemon(string pokemonName); 
    }
}
