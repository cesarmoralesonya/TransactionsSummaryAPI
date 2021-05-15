using Infraestructure.ApiClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.ApiClients.RateClientTests
{
    public class GetAll
    {
        private readonly RateClient _rateClient;

        private readonly ITestOutputHelper _output;
        
        private static IConfiguration configuration;

        private static Mock<ILogger<RateClient>> loggerMockClient;

        public GetAll(ITestOutputHelper output)
        {
            _output = output;

            var client = new HttpClient() { BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/") };
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(httpClient => httpClient.CreateClient(It.IsAny<string>())).Returns(client);

            string projectPath = AppDomain
                                        .CurrentDomain
                                        .BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            configuration = new ConfigurationBuilder()
                                .SetBasePath(projectPath)
                                .AddJsonFile("appsettings.json")
                                .Build();

            loggerMockClient = new Mock<ILogger<RateClient>>();

            _rateClient = new RateClient(mockFactory.Object, loggerMockClient.Object, configuration);
        }

        [Fact]
        public async Task NotNull()
        {
            //Acts
            var result = await _rateClient.GetAllAsync();

            //Asserts
            Assert.NotNull(result);
        }
    }
}
