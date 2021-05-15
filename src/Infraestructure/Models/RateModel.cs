using System.Text.Json.Serialization;

namespace Infraestructure.Models
{
    public class RateModel
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("rate")]
        public double Rate { get; set; }
    }
}
