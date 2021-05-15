using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.ApiClients.TransactionClient
{
    public class GetAll
    {
        private readonly Infraestructure.ApiClients.TransactionClient _rateClient;

        private readonly ITestOutputHelper _output;

        public GetAll(ITestOutputHelper output)
        {
            _output = output;

            var client = new HttpClient() { BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/") };
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(httpClient => httpClient.CreateClient(It.IsAny<string>())).Returns(client);

            _rateClient = new Infraestructure.ApiClients.TransactionClient(mockFactory.Object);
        }

        [Fact]
        public async Task NotNull()
        {
            //Acts
            var result = await _rateClient.GetAll();

            //Asserts
            Assert.NotNull(result);
        }
    }
}
