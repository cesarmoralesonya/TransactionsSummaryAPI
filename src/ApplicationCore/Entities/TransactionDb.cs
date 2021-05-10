using ApplicationCore.Interfaces;
using System;

namespace ApplicationCore.Entities
{
    public class TransactionDb : BaseEntity, IAgragateRoot
    {
        public string Sku { get; private set; }

        public double Amount { get; private set; }

        public string Currency { get; private set; }

        public TransactionDb()
        {
            //Required by EF
        }

        public TransactionDb(string sku, double amount, string currency)
        {
            Sku = sku ?? throw new ArgumentNullException(nameof(sku));
            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }
    }
}
