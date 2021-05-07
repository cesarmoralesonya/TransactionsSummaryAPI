using ApplicationCore.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class TransactionsFilterSpecification : Specification<Transaction>
    {
        public TransactionsFilterSpecification(string sku): base()
        {
            Query.Where(i => i.Sku == sku);
        }
    }
}
