using Infraestructure.ApiClients;
using Infraestructure.Models;
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

namespace UnitTests.Infraestructure.ApiClients.TransactionClientTests
{
    public class GetAll
    {
        private static IConfiguration configuration;

        private static Mock<ILogger<TransactionClient>> loggerMockClient;

        public GetAll()
        {
            string projectPath = AppDomain
                                        .CurrentDomain
                                        .BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            configuration = new ConfigurationBuilder()
                                .SetBasePath(projectPath)
                                .AddJsonFile("appsettings.json")
                                .Build();

            loggerMockClient = new Mock<ILogger<TransactionClient>>();
        }

        [Fact]
        public async Task Null()
        {
            //Arrange
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest });

            var client = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/") };
            mockFactory.Setup(httpClient => httpClient.CreateClient(It.IsAny<string>())).Returns(client);

            var transactionClient = new TransactionClient(mockFactory.Object, loggerMockClient.Object, configuration);

            //Act
            var result = await transactionClient.GetAllAsync();

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task NotNullTypeAndCountCorrect()
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

            //Act
            var result = await transactionClient.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<TransactionModel>>(result);
            Assert.Equal(5, result.ToList().Count);
        }
    }
}
