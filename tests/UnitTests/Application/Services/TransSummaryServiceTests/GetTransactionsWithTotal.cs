//using Application.Mappings;
//using Application.Services;
//using AutoMapper;
//using Domain.Entities;
//using Infraestructure.ApiClients;
//using Infraestructure.Data.Repositories;
//using Infraestructure.Interfaces;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Moq.Protected;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace UnitTests.Application.Services.TransSummaryServiceTests
//{
//    public class GetTransactionsWithTotal
//    {
//        private static IMapper _mapper;

//        private static IConfiguration configuration;

//        private static Mock<ILogger<TransactionClient>> loggerMockTransactionClient;
//        private static Mock<ILogger<TransactionRepository>> loggerMockTransactionRepository;

//        private static Mock<ILogger<RateClient>> loggerMockRateClient;
//        private static Mock<ILogger<RateRepository>> loggerMockRateRepository;

//        private static Mock<ILogger<TransSummaryService>> loggerMockService;

//        public GetTransactionsWithTotal()
//        {
//            if (_mapper == null)
//            {
//                var mappingConfig = new MapperConfiguration(mc =>
//                {
//                    mc.AddProfile(new MappingProfile());
//                });
//                IMapper mapper = mappingConfig.CreateMapper();
//                _mapper = mapper;
//            }
//            string projectPath = AppDomain
//                                       .CurrentDomain
//                                       .BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
//            configuration = new ConfigurationBuilder()
//                                .SetBasePath(projectPath)
//                                .AddJsonFile("appsettings.json")
//                                .Build();

//            loggerMockTransactionClient = new Mock<ILogger<TransactionClient>>();
//            loggerMockTransactionRepository = new Mock<ILogger<TransactionRepository>>();

//            loggerMockRateClient = new Mock<ILogger<RateClient>>();
//            loggerMockRateRepository = new Mock<ILogger<RateRepository>>();
//            loggerMockService = new Mock<ILogger<TransSummaryService>>();
//        }

//        [Fact]
//        public async Task GetTransactionsWithTotalFromApi()
//        {
//            //Arrange
//            var contentTransactions = new StringContent("[{\"sku\":\"T2006\",\"amount\":\"10.00\",\"currency\":\"USD\"},{\"sku\":\"M2007\",\"amount\":\"34.57\",\"currency\":\"CAD\"},{\"sku\":\"R2008\",\"amount\":\"17.95\",\"currency\":\"USD\"},{\"sku\":\"T2006\",\"amount\":\"7.63\",\"currency\":\"EUR\"},{\"sku\":\"B2009\",\"amount\":\"21.23\",\"currency\":\"USD\"}]");
//            var contentRates = new StringContent("[{\"from\":\"EUR\",\"to\":\"USD\",\"rate\":\"1.359\"},{\"from\":\"CAD\",\"to\":\"EUR\", \"rate\":\"0.732\"},{\"from\":\"USD\",\"to\":\"EUR\",\"rate\":\"0.736\"},{\"from\":\"EUR\",\"to\":\"CAD\",\"rate\":\"1.366\"}]");

//            var mockFactoryTransactions = new Mock<IHttpClientFactory>();
//            var mockFactoryRates = new Mock<IHttpClientFactory>();

//            var mockHttpMessageHandlerTransactions = new Mock<HttpMessageHandler>();
//            mockHttpMessageHandlerTransactions.Protected()
//                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
//                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = contentTransactions });

//            var mockHttpMessageHandlerRates = new Mock<HttpMessageHandler>();
//            mockHttpMessageHandlerRates.Protected()
//                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
//                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = contentRates })
//                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = contentRates });

//            var clientTransactions = new HttpClient(mockHttpMessageHandlerTransactions.Object) { BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/") };
//            mockFactoryTransactions.Setup(httpClient => httpClient.CreateClient(It.IsAny<string>()))
//                                    .Returns(clientTransactions);

//            var clientRates = new HttpClient(mockHttpMessageHandlerRates.Object) { BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/") };
//            mockFactoryRates.SetupSequence(httpClient => httpClient.CreateClient(It.IsAny<string>()))
//                                .Returns(clientRates)
//                                .Returns(clientRates);

//            var transactionClient = new TransactionClient(mockFactoryTransactions.Object, loggerMockTransactionClient.Object, configuration);
//            var rateClient = new RateClient(mockFactoryRates.Object, loggerMockRateClient.Object, configuration);

//            var mockRepoTransactions = new Mock<ITransactionRepository>();
//            mockRepoTransactions.Setup(repo => repo.DeleteAllAsync(It.IsAny<CancellationToken>()));
//            mockRepoTransactions.Setup(repo => repo.AddRangeAsync(It.IsAny<IEnumerable<TransactionEntity>>(), It.IsAny<CancellationToken>()));

//            var mockRepoRates = new Mock<IRateRepository>();
//            mockRepoRates.Setup(repo => repo.DeleteAllAsync(It.IsAny<CancellationToken>()));
//            mockRepoRates.Setup(repo => repo.AddRangeAsync(It.IsAny<IEnumerable<RateEntity>>(), It.IsAny<CancellationToken>()));

//            var transSummaryService = new TransSummaryService(_mapper, rateClient, mockRepoRates.Object, transactionClient, mockRepoTransactions.Object, loggerMockService.Object);

//            //Act
//            var transactionTotalDto = await transSummaryService.GetTransactionsWithTotal("T2006");

//            //Assert
//            Assert.Equal(14.99M, transactionTotalDto.Total);
//        }
//    }
//}
