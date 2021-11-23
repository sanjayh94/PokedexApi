using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PokedexApi.Models
{
    public class PokemonTranslated
    {
        // API Response Status for internal app use to determine if Service returned Not Found or 500 errors. Initialising to worst case scenario.
        public HttpStatusCode ApiResponseStatus { get; set; } = HttpStatusCode.InternalServerError;

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("habitat")]
        public string Habitat { get; set; }

        [JsonProperty("isLegendary")]
        public bool IsLegendary { get; set; }
    }
}
