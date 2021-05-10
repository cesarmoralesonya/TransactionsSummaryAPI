using Newtonsoft.Json;
using PublicApi.Dtos;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.PublicApi.Controllers
{
    [Collection("SequentialGet")]
    public class ConversionsControllerGet : IClassFixture<WebTestFixture>
    {
        public ConversionsControllerGet(WebTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; }

        [Fact]
        public async Task RetunsListConversions()
        {
            var response = await Client.GetAsync("/api/conversions");
            response.EnsureSuccessStatusCode();
            var stringResponde = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<ConversionDto>>(stringResponde);

            Assert.NotEmpty(model);
        }
    }
}
