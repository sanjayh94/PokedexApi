using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PokedexApi.Models
{
    // To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
    //
    //    using PokedexApi.Models;
    //
    //    var pokemon = Pokemon.FromJson(jsonString);

    public partial class Pokemon
    {
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
        public Info Species { get; set; }

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

    public partial class Pokemon
    {
        public static Pokemon FromJson(string json) => JsonConvert.DeserializeObject<Pokemon>(json, PokedexApi.Models.Converter.Settings);
    }

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
