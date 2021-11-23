using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PokedexApi.Models
{
    public partial class TranslationResponse
    {
        [JsonProperty("success")]
        public Success Success { get; set; }

        [JsonProperty("contents")]
        public Contents Contents { get; set; }

        [JsonProperty("error")]
        public Error Error { get; set; }
    }

    public class Contents
    {
        [JsonProperty("translated")]
        public string Translated { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("translation")]
        public string Translation { get; set; }
    }

    public class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }
    }

    public class Success
    {
        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public partial class TranslationResponse
    {
        public static TranslationResponse FromJson(string json) => JsonConvert.DeserializeObject<TranslationResponse>(json, PokedexApi.Models.Converter.Settings);
    }

}
