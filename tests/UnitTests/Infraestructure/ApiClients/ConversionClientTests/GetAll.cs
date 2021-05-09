using ApplicationCore.Entities;
using Infraestructure.ApiClients;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Infraestructure.ApiClients.ConversionClientTests
{
    public class GetAll
    {
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

            var conversionClient = new ConversionClient(mockFactory.Object);

            //Act
            var result = await conversionClient.GetAll();

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task NotNull_TypeCorrect()
        {
            //Arrange
            var content = new StringContent("[{ 'from': 'EUR', 'to': 'USD', 'rate': '1.359' },{ 'from': 'CAD', 'to': 'EUR', 'rate': '0.732' },]");
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = content });

            var client = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/") };
            mockFactory.Setup(httpClient => httpClient.CreateClient(It.IsAny<string>())).Returns(client);

            var conversionClient = new ConversionClient(mockFactory.Object);

            //Act
            var result = await conversionClient.GetAll();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Conversion>>(result);
        }
    }
}
