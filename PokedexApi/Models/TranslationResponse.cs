using Newtonsoft.Json;

namespace PokedexApi.Models
{
    /// <summary>
    /// Model Class for recieving API response from the translator API
    /// </summary>
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

    /// <summary>
    /// Helper method to Deserialize json string to the .Net TranslationResponse model class
    /// </summary>
    public partial class TranslationResponse
    {
        public static TranslationResponse FromJson(string json) => JsonConvert.DeserializeObject<TranslationResponse>(json, PokedexApi.Models.Converter.Settings);
    }

}
