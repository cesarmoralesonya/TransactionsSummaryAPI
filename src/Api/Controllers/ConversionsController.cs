using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionsController : ControllerBase
    {
        private readonly IConversionService _conversionService;

        public ConversionsController(IConversionService conversionService)
        {
            _conversionService = conversionService ?? throw new ArgumentNullException(nameof(conversionService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConversions()
        {
            var conversions = await _conversionService.GetAllConversionsAsync();
            return Ok(conversions);
        }
    }
}
