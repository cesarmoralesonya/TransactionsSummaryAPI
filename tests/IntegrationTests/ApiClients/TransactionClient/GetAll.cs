using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.ApiClients.TransactionClient
{
    public class GetAll
    {
        private readonly Infraestructure.ApiClients.TransactionClient _conversionClient;

        private readonly ITestOutputHelper _output;

        public GetAll(ITestOutputHelper output)
        {
            _output = output;

            var client = new HttpClient() { BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/") };
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(httpClient => httpClient.CreateClient(It.IsAny<string>())).Returns(client);

            _conversionClient = new Infraestructure.ApiClients.TransactionClient(mockFactory.Object);
        }

        [Fact]
        public async Task NotNull()
        {
            //Acts
            var result = await _conversionClient.GetAll();

            //Asserts
            Assert.NotNull(result);
        }
    }
}
