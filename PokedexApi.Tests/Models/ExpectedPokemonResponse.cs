using Newtonsoft.Json;

namespace PokedexApi.Tests.Models
{
    class ExpectedPokemonResponse
    {
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
