using Newtonsoft.Json;

namespace Infraestructure.Models
{
    public class RateModel
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }
    }
}
