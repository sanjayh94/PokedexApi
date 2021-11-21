using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PokedexApi.Models
{
    public class PokemonDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("habitat")]
        public string Habitat { get; set; }

        [JsonPropertyName("isLegendary")]
        public string IsLegendary { get; set; }
    }
}
