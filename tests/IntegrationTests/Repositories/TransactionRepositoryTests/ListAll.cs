using Infraestructure.Data;
using Infraestructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Repositories.TransactionRepositoryTests
{
    public class ListAll
    {
        private readonly TransSummaryContext _context;

        private readonly TransactionRepository _transactionRepository;

        public ListAll()
        {
            var dbOptions = new DbContextOptionsBuilder<TransSummaryContext>()
                .UseInMemoryDatabase(databaseName: "TestListAll")
                .Options;
            _context = new TransSummaryContext(dbOptions);
            TransSummaryContextSeed.Initialize(_context);
            _transactionRepository = new TransactionRepository(_context);
        }

        [Fact]
        public async Task NotNull()
        {
            //Act
            var listTransactions = await _transactionRepository.ListAllAsync();

            //Arrange
            Assert.NotNull(listTransactions);

        }
    }
}
