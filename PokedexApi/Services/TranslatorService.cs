using Microsoft.Extensions.Logging;
using PokedexApi.Interfaces;
using PokedexApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexApi.Services
{
    public class TranslatorService : ITranslatorService
    {
        private readonly ILogger<TranslatorService> _logger;
        
        public TranslatorService(ILogger<TranslatorService> logger)
        {
            _logger = logger;
        }

        public PokemonDTO GetTranslatedPokemonDescriptionWithConditions(string pokemonName)
        {
            throw new NotImplementedException();
        }
    }
}
