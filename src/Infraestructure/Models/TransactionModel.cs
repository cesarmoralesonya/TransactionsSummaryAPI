using Newtonsoft.Json;

namespace Infraestructure.Models
{
    public class TransactionModel
    {
        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
