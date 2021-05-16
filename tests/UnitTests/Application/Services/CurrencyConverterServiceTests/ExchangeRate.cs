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
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Builder;
using Xunit;

namespace UnitTests.Application.Services.CurrencyConverterServiceTests
{
    public class ExchangeRate
    {
        private static IMapper _mapper;

        private static IConfiguration configuration;

        private static Mock<ILogger<RateClient>> loggerMockRateClient;

        private static Mock<ILogger<CurrencyConverterService>> loggerMockService;

        public ExchangeRate()
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

            loggerMockRateClient = new Mock<ILogger<RateClient>>();
            loggerMockService = new Mock<ILogger<CurrencyConverterService>>();
        }

        [Fact]
        public async Task GetExchangeRateFromApi()
        {
            //Arrange
            var content = ContentBuilder.RatesContent();
            var mockFactoryRates = new Mock<IHttpClientFactory>();

            var mockHttpMessageHandlerRates = new Mock<HttpMessageHandler>();
            mockHttpMessageHandlerRates.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = content });

            var clientRates = new HttpClient(mockHttpMessageHandlerRates.Object) { BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/") };
            mockFactoryRates.Setup(httpClient => httpClient.CreateClient(It.IsAny<string>()))
                                .Returns(clientRates);

            var rateClient = new RateClient(mockFactoryRates.Object, loggerMockRateClient.Object, configuration);

            var mockRepoRates = new Mock<IRateRepository>();
            mockRepoRates.Setup(repo => repo.DeleteAllAsync(It.IsAny<CancellationToken>()));
            mockRepoRates.Setup(repo => repo.AddRangeAsync(It.IsAny<IEnumerable<RateEntity>>(), It.IsAny<CancellationToken>()));

            var service = new CurrencyConverterService(_mapper, rateClient, mockRepoRates.Object, loggerMockService.Object);

            //Act
            await service.Initialization;
            var rate = service.ExchangeRate("USD", "CAD");

            //Assert
            Assert.Equal(1.005376M, rate);
        }
    }
}
