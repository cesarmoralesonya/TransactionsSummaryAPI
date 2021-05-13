using Infraestructure.Interfaces;
using System.Text.Json.Serialization;

namespace Infraestructure.Models
{
    public class TransactionModel : IWebServiceModel
    {
        public string Sku { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }
    }
}
