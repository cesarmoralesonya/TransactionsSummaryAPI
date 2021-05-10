using ApplicationCore.Interfaces;
using System;

namespace ApplicationCore.Entities
{
    public class TransactionEntity : BaseEntity, IAgragateRoot
    {
        public string Sku { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public TransactionEntity()
        {
            //Required by EF
        }

        public TransactionEntity(string sku, double amount, string currency)
        {
            Sku = sku ?? throw new ArgumentNullException(nameof(sku));
            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }
    }
}
