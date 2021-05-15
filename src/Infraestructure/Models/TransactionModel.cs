using System.Text.Json.Serialization;

namespace Infraestructure.Models
{
    public class TransactionModel
    {
        [JsonPropertyName("sku")]
        public string Sku { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}
