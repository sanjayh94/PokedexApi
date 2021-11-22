using Newtonsoft.Json;
using System.Collections.Generic;

namespace PokedexApi.Models
{
    // To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
    //
    //    using PokedexApi.Models;
    //
    //    var pokemonSpecies = PokemonSpecies.FromJson(jsonString);
    public partial class PokemonSpecies
    {
        [JsonProperty("base_happiness")]
        public long BaseHappiness { get; set; }

        [JsonProperty("capture_rate")]
        public long CaptureRate { get; set; }

        [JsonProperty("flavor_text_entries")]
        public List<FlavorTextEntry> FlavorTextEntries { get; set; }

        [JsonProperty("form_descriptions")]
        public List<object> FormDescriptions { get; set; }

        [JsonProperty("forms_switchable")]
        public bool FormsSwitchable { get; set; }

        [JsonProperty("gender_rate")]
        public long GenderRate { get; set; }

        [JsonProperty("generation")]
        public Info Generation { get; set; }

        [JsonProperty("growth_rate")]
        public Info GrowthRate { get; set; }

        [JsonProperty("habitat")]
        public Info Habitat { get; set; }

        [JsonProperty("has_gender_differences")]
        public bool HasGenderDifferences { get; set; }

        [JsonProperty("hatch_counter")]
        public long HatchCounter { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("is_baby")]
        public bool IsBaby { get; set; }

        [JsonProperty("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonProperty("is_mythical")]
        public bool IsMythical { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public long Order { get; set; }

        [JsonProperty("shape")]
        public Info Shape { get; set; }

        [JsonProperty("varieties")]
        public List<Variety> Varieties { get; set; }
    }

    public partial class FlavorTextEntry
    {
        [JsonProperty("flavor_text")]
        public string FlavorText { get; set; }

        [JsonProperty("language")]
        public Info Language { get; set; }

        [JsonProperty("version")]
        public Info Version { get; set; }
    }    

    public partial class Variety
    {
        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("pokemon")]
        public Info Pokemon { get; set; }
    }

    public partial class PokemonSpecies
    {
        public static PokemonSpecies FromJson(string json) => JsonConvert.DeserializeObject<PokemonSpecies>(json, PokedexApi.Models.Converter.Settings);
    }

}
