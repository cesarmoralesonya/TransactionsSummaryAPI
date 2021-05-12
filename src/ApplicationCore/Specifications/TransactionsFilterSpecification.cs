using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
    public class TransactionsFilterSpecification : Specification<TransactionEntity>
    {
        public TransactionsFilterSpecification(string sku) : base()
        {
            Query.Where(i => i.Sku == sku);
        }
    }
}
