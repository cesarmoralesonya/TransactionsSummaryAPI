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
    [Route("api/rates")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IRateService _rateService;

        public RatesController(IRateService rateService, ILogger<RatesController> logger)
        {
            _rateService = rateService ?? throw new ArgumentNullException(nameof(rateService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all rates
        /// </summary>
        /// <response code="200">Return a list of rates</response>
        /// <response code="404">Not fount rates</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllrates(CancellationToken cancellationToken)
        {
            try
            {
                var rates = await _rateService.GetAllratesAsync(cancellationToken);
                if (rates == null) return NotFound();
                return Ok(rates);
            }
            catch (Exception ex)
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
