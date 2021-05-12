using Application.Mappings;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infraestructure.ApiClients;
using Infraestructure.Interfaces;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ApplicationCore.Services.TransactionServiceTests
{
    public class GetAllTransactions
    {
        private static IMapper _mapper;
        public GetAllTransactions()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public async Task GetAllTransactionsFromDb()
        {
            //Arrange
            var mockRepo = new Mock<ITransactionRepository>();
            mockRepo.Setup(repo => repo.ListAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetTransactions());

            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest });
            var client = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/") };
            mockFactory.Setup(httpClient => httpClient.CreateClient(It.IsAny<string>())).Returns(client);
            var transactionClient = new TransactionClient(mockFactory.Object);

            var transactionService = new TransactionService(_mapper, mockRepo.Object, transactionClient);

            //Act
            var transactions = await transactionService.GetAllTransactionsAsync();

            //Assets
            Assert.NotNull(transactions);
            Assert.Equal(GetTransactions().Count(), transactions.ToList().Count());
        }

        [Fact]
        public async Task GetAllTransactionsFromApi()
        {
            //Arrange
            var content = new StringContent("[{ 'sku':'H7277','amount':'18.1','currency':'AUD'},{ 'sku':'E0349','amount':'23.9','currency':'EUR'},{ 'sku':'M0183','amount':'16.0','currency':'CAD'},]");
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = content });
            var client = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/") };
            mockFactory.Setup(httpClient => httpClient.CreateClient(It.IsAny<string>())).Returns(client);
            var transactionClient = new TransactionClient(mockFactory.Object);

            var mockRepo = new Mock<ITransactionRepository>();
            mockRepo.Setup(repo => repo.DeleteAllAsync(It.IsAny<CancellationToken>()));
            mockRepo.Setup(repo => repo.AddRangeAsync(It.IsAny<IEnumerable<TransactionEntity>>(), It.IsAny<CancellationToken>()));

            var transactionService = new TransactionService(_mapper, mockRepo.Object, transactionClient);

            //Act
            var transactions = await transactionService.GetAllTransactionsAsync();

            //Asserts
            Assert.NotNull(transactions);
            Assert.Equal(3, transactions.ToList().Count);

        }

        #region snippet_GetTestTransactions
        private List<TransactionEntity> GetTransactions()
        {
            return new List<TransactionEntity>()
            {
                new TransactionEntity() { Sku = "T2006", Amount =  10.0, Currency =  "USD"},
                new TransactionEntity() { Sku = "T2006", Amount = 20.0, Currency = "EUR"},
                new TransactionEntity() { Sku = "T2008", Amount = 30.0, Currency = "USD"},
                new TransactionEntity() { Sku = "T2008", Amount = 5.0, Currency = "EUR"},
                new TransactionEntity() { Sku = "T2008", Amount = 8.0, Currency = "EUR"},
            };
        }
        #endregion
    }
}
