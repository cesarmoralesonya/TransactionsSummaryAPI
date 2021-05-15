using Newtonsoft.Json;

namespace Infraestructure.Models
{
    public class TransactionModel
    {
        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
