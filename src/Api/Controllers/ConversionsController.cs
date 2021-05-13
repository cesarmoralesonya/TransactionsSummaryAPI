using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.Controllers
{
    [Produces("application/json")]
    [Route("api/conversions")]
    [ApiController]
    public class ConversionsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConversionService _conversionService;

        public ConversionsController(IConversionService conversionService, ILogger<ConversionsController> logger)
        {
            _conversionService = conversionService ?? throw new ArgumentNullException(nameof(conversionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all Conversions
        /// </summary>
        /// <response code="200">Return a list of conversions</response>
        /// <response code="404">Not fount conversions</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllConversions(CancellationToken cancellationToken)
        {
            try
            {
                var conversions = await _conversionService.GetAllConversionsAsync(cancellationToken);
                if (conversions == null) return NotFound();
                return Ok(conversions);
            }
            catch(Exception ex)
            {
                var menssage = "Error message: " + ex.Message;

                if (ex.InnerException != null)
                {
                    menssage += " Inner exception: " + ex.InnerException.Message;
                }

                menssage += " Stack trace: " + ex.StackTrace;
                _logger.LogCritical(menssage);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
