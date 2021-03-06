using Application.Mappings;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infraestructure.ApiClients;
using Infraestructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Builder;
using Xunit;

namespace UnitTests.Application.Services.TransactionServiceTests
{
    public class GetAllTransactions
    {
        private static IMapper _mapper;

        private static IConfiguration configuration;

        private static Mock<ILogger<TransactionClient>> loggerMockClient;
        private static Mock<ILogger<TransactionService>> loggerMockService;

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
            string projectPath = AppDomain
                                       .CurrentDomain
                                       .BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            configuration = new ConfigurationBuilder()
                                .SetBasePath(projectPath)
                                .AddJsonFile("appsettings.json")
                                .Build();

            loggerMockClient = new Mock<ILogger<TransactionClient>>();
            loggerMockService = new Mock<ILogger<TransactionService>>();
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


            var transactionClient = new TransactionClient(mockFactory.Object, loggerMockClient.Object, configuration);
            var transactionService = new TransactionService(_mapper, mockRepo.Object, transactionClient, loggerMockService.Object);

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
            var content = ContentBuilder.TransactionsContent();
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = content });

            var client = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/") };
            mockFactory.Setup(httpClient => httpClient.CreateClient(It.IsAny<string>())).Returns(client);

            var transactionClient = new TransactionClient(mockFactory.Object, loggerMockClient.Object, configuration);

            var mockRepo = new Mock<ITransactionRepository>();
            mockRepo.Setup(repo => repo.DeleteAllAsync(It.IsAny<CancellationToken>()));
            mockRepo.Setup(repo => repo.AddRangeAsync(It.IsAny<IEnumerable<TransactionEntity>>(), It.IsAny<CancellationToken>()));

            var transactionService = new TransactionService(_mapper, mockRepo.Object, transactionClient, loggerMockService.Object);

            //Act
            var transactions = await transactionService.GetAllTransactionsAsync();

            //Asserts
            Assert.NotNull(transactions);
            Assert.Equal(5, transactions.ToList().Count);

        }

        #region snippet_GetTestTransactions
        private List<TransactionEntity> GetTransactions()
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
        #endregion
    }
}
