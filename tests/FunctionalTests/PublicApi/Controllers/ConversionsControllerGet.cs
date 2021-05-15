using Application.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.PublicApi.Controllers
{
    [Collection("SequentialGet")]
    public class ratesControllerGet : IClassFixture<WebTestFixture>
    {
        public ratesControllerGet(WebTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; }

        [Fact]
        public async Task RetunsListrates()
        {
            var response = await Client.GetAsync("/api/rates");
            response.EnsureSuccessStatusCode();
            var stringResponde = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<RateDto>>(stringResponde);

            Assert.NotEmpty(model);
        }
    }
}
