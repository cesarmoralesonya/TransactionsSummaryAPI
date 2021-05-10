using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionsController : ControllerBase
    {
        private readonly IConversionClient<IWebServicesEntity> _conversionClient;
        private readonly IMapper _mapper;

        public ConversionsController(IConversionClient<IWebServicesEntity> conversionClient, IMapper mapper)
        {
            _conversionClient = conversionClient ?? throw new ArgumentNullException(nameof(conversionClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetConversions()
        {
            var items = await _conversionClient.GetAll();
            var response = _mapper.Map<IEnumerable<ConversionDto>>(items);

            return Ok(response);
        }
    }
}
