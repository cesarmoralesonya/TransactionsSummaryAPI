using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class Transaction: IWebServicesEntity
    {
        public string Sku { get; private set; }

        public decimal Amount { get; private set; }

        public string Currency { get; private set; }
    }
}
