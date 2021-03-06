using Domain.Entities;
using Domain.Specifications;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests.Domain.Specifications
{
    public class TransactionsFilterEspecificationTest
    {
        [Theory]
        [InlineData("T2006", 2)]
        [InlineData("T2008", 3)]
        public void MatchesExpectedNumberOfTransactions(string sku, int expectedCount)
        {
            var spec = new TransactionsFilterSpecification(sku);

            var result = GetTestTransactionsCollection()
                            .AsQueryable()
                            .Where(spec.WhereExpressions.FirstOrDefault());

            Assert.Equal(expectedCount, result.Count());


        }

        public List<TransactionEntity> GetTestTransactionsCollection()
        {
            return new List<TransactionEntity>()
            {
                new TransactionEntity() { Sku = "T2006", Amount =  10.0M, Currency =  "USD"},
                new TransactionEntity() { Sku = "T2006", Amount = 20.0M, Currency = "EUR"},
                new TransactionEntity() { Sku = "T2008", Amount = 30.0M, Currency = "USD"},
                new TransactionEntity() { Sku = "T2008", Amount = 5.0M, Currency = "EUR"},
                new TransactionEntity() { Sku = "T2008", Amount = 8.0M, Currency = "EUR"},
            };
        }
    }
}
