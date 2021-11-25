using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace PokedexApi.Models
{
    // To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
    //
    //    using PokedexApi.Models;
    //
    //    var pokemon = Pokemon.FromJson(jsonString);

    /// <summary>
    /// Model Data class for Pokemon related Data
    /// </summary>
    public partial class Pokemon
    {
        // API Response Status for internal app use to determine if Service returned Not Found or 500 errors. Initialising to worst case scenario.
        public HttpStatusCode ApiResponseStatus { get; set; } = HttpStatusCode.InternalServerError;

        [JsonProperty("abilities")]
        public List<Ability> Abilities { get; set; }

        [JsonProperty("base_experience")]
        public long BaseExperience { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("location_area_encounters")]
        public Uri LocationAreaEncounters { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public long Order { get; set; }

        [JsonProperty("species")]
        public PokemonSpecies Species { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }
    }

    public partial class Ability
    {
        [JsonProperty("ability")]
        public Info AbilityInfo { get; set; }

        [JsonProperty("is_hidden")]
        public bool IsHidden { get; set; }

        [JsonProperty("slot")]
        public long Slot { get; set; }
    }

    public partial class Info
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    /// <summary>
    /// Helper method to Deserialize json string to the .Net Pokemon model class
    /// </summary>
    public partial class Pokemon
    {
        public static Pokemon FromJson(string json) => JsonConvert.DeserializeObject<Pokemon>(json, PokedexApi.Models.Converter.Settings);
    }

    /// <summary>
    /// Helper method to Serialize this class to json string
    /// </summary>
    public static class Serialize
    {
        public static string ToJson(this Pokemon self) => JsonConvert.SerializeObject(self, PokedexApi.Models.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
