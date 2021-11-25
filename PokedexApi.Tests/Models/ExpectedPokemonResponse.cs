using Newtonsoft.Json;

namespace PokedexApi.Tests.Models
{
    /// <summary>
    /// Model class to use as a comparison for expected Pokemon response from the API
    /// </summary>
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
