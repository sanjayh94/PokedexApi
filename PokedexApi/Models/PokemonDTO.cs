using Newtonsoft.Json;

namespace PokedexApi.Models
{
    /// <summary>
    /// Model Data class for Data Transfer Object. This is the class used to send final data to User.
    /// </summary>
    public class PokemonDTO
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
