using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTests.ApplicationCore.Specifications
{
    public class TransactionsFilterEspecification
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

        public List<Transaction> GetTestTransactionsCollection()
        {
            return new List<Transaction>()
            {
                new Transaction("T2006", 10.0, "USD"),
                new Transaction("T2006", 20.0, "EUR"),
                new Transaction("T2008", 30.0, "USD"),
                new Transaction("T2008", 5.0, "EUR"),
                new Transaction("T2008", 8.0, "EUR"),
            };
        }
    }
}
