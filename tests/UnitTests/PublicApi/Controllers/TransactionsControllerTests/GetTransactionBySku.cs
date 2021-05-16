using Application.Dtos;
using Application.Mappings;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infraestructure.ApiClients;
using Infraestructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using PublicApi.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Builder;
using Xunit;

namespace UnitTests.PublicApi.Controllers.TransactionsControllerTests
{
    public class GetTransactionBySku
    {

        private static Mock<ILogger<TransactionsController>> logger;
        private static Mock<ILogger<RateClient>> loggerMockRateClient;
        private static Mock<ILogger<CurrencyConverterService>> loggerCurrencyConverter;
        private static Mock<ILogger<TransactionClient>> loggerMockTransactionClient;
        private static Mock<ILogger<TransactionService>> loggerMockTransactionService;

        private static IMapper _mapper;

        private static IConfiguration configuration;

        public GetTransactionBySku()
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

            logger = new Mock<ILogger<TransactionsController>>();
            loggerMockRateClient = new Mock<ILogger<RateClient>>();
            loggerMockTransactionClient = new Mock<ILogger<TransactionClient>>();
            loggerCurrencyConverter = new Mock<ILogger<CurrencyConverterService>>();
            loggerMockTransactionService = new Mock<ILogger<TransactionService>>();
        }

        [Fact]
        public async Task TotalCorrect()
        {
            //Arrange
            var contentRates = ContentBuilder.RatesContent();
            var mockFactoryRates = new Mock<IHttpClientFactory>();

            var mockHttpMessageHandlerRates = new Mock<HttpMessageHandler>();
            mockHttpMessageHandlerRates.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = contentRates });

            var clientRates = new HttpClient(mockHttpMessageHandlerRates.Object) { BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/") };
            mockFactoryRates.Setup(httpClient => httpClient.CreateClient(It.IsAny<string>()))
                                .Returns(clientRates);

            var rateClient = new RateClient(mockFactoryRates.Object, loggerMockRateClient.Object, configuration);

            var mockRepoRates = new Mock<IRateRepository>();
            mockRepoRates.Setup(repo => repo.DeleteAllAsync(It.IsAny<CancellationToken>()));
            mockRepoRates.Setup(repo => repo.AddRangeAsync(It.IsAny<IEnumerable<RateEntity>>(), It.IsAny<CancellationToken>()));

            var currencyConverterService = new CurrencyConverterService(_mapper, rateClient, mockRepoRates.Object, loggerCurrencyConverter.Object);

            var contentTransactions = ContentBuilder.TransactionsContent();
            var mockFactoryTransactions = new Mock<IHttpClientFactory>();

            var mockHttpMessageHandlerTransactions = new Mock<HttpMessageHandler>();
            mockHttpMessageHandlerTransactions.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = contentTransactions });

            var clientTransactions = new HttpClient(mockHttpMessageHandlerTransactions.Object) { BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/") };
            mockFactoryTransactions.Setup(httpClient => httpClient.CreateClient(It.IsAny<string>()))
                                    .Returns(clientTransactions);


            var transactionClient = new TransactionClient(mockFactoryTransactions.Object, loggerMockTransactionClient.Object, configuration);

            var mockRepoTransactions = new Mock<ITransactionRepository>();
            mockRepoTransactions.Setup(repo => repo.DeleteAllAsync(It.IsAny<CancellationToken>()));
            mockRepoTransactions.Setup(repo => repo.AddRangeAsync(It.IsAny<IEnumerable<TransactionEntity>>(), It.IsAny<CancellationToken>()));

            var transactionService = new TransactionService(_mapper, mockRepoTransactions.Object, transactionClient, loggerMockTransactionService.Object);


            var controller = new TransactionsController(transactionService, logger.Object, currencyConverterService);

            //Act
            var result = await controller.GetTransactionsBySku("T2006");

            //Asserts
            var createdAtAction = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<TransactionsTotalDto>(createdAtAction.Value);
            Assert.Equal(14.99M, value.Total);

        }
    }
}
