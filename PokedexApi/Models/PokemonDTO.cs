using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexApi.Models
{
    public class PokemonDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("habitat")]
        public string Habitat { get; set; }

        [JsonProperty("isLegendary")]
        public string IsLegendary { get; set; }
    }
}
