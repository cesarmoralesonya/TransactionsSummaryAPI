using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PublicApi;
using PublicApi.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.PublicApi.Controllers
{
    public class ConversionControllerTests
    {
        public static IMapper _mapper;
        public ConversionControllerTests()
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
        }

        [Fact]
        public async Task ReturnAllConversions()
        {
            //Arrange
            var mockConversionClient = new Mock<IConversionClient<IWebServicesEntity>>();
            mockConversionClient.Setup(client => client.GetAll()).ReturnsAsync(GetConversions());
            var controller = new ConversionsController(mockConversionClient.Object, _mapper);

            //Act
            var result = await controller.GetConversions();

            //Asserts
            Assert.IsType<OkObjectResult>(result);

        }

        #region snippet_GetConversions
        private List<Conversion> GetConversions()
        {
            return new List<Conversion>()
            {
                new Conversion() { From = "EUR", To = "USD", Rate = 1.359m },
                new Conversion() { From = "CAD", To = "EUR", Rate = 0.732m }
            };
        }
        #endregion
    }
}
