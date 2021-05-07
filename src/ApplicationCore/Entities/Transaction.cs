using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class Transaction: IWebServicesEntity
    {
        public string Sku { get; private set; }

        public double Amount { get; private set; }

        public string Currency { get; private set; }

        public Transaction(string sku, double amount, string currency)
        {
            Sku = sku;
            Amount = amount;
            Currency = currency;
        }
    }
}
