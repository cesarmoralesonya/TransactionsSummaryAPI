using Domain.Interfaces;
using System;

namespace Domain.Entities
{
    public class TransactionEntity : EntityBase, IAgragateRoot
    {
        public string Sku { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public TransactionEntity()
        {
            //Required by EF
        }

        public TransactionEntity(string sku, decimal amount, string currency)
        {
            Sku = sku ?? throw new ArgumentNullException(nameof(sku));
            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }
    }
}
