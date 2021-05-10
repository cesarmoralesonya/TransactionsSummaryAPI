using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class TransactionModel : IWebServicesEntity
    {
        public string Sku { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }
    }
}
