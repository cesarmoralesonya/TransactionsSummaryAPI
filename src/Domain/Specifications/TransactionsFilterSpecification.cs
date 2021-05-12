using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications
{
    public class TransactionsFilterSpecification : Specification<TransactionEntity>
    {
        public TransactionsFilterSpecification(string sku) : base()
        {
            Query.Where(i => i.Sku == sku);
        }
    }
}
