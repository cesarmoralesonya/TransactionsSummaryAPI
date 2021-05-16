using Application.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.PublicApi.ControllersTests
{
    [Collection("SequentialGet")]
    public class RatesControllerGet : IClassFixture<WebTestFixture>
    {
        public RatesControllerGet(WebTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; }

        [Fact]
        public async Task RetunsListRates()
        {
            var response = await Client.GetAsync("/api/rates");
            response.EnsureSuccessStatusCode();
            var stringResponde = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<RateDto>>(stringResponde);

            Assert.NotEmpty(model);
        }
    }
}
