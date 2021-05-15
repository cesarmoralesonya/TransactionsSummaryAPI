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
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ITransactionService _transactionService;
        private readonly ITransSummaryService _transSummaryService;

        public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger, ITransSummaryService transSummaryService)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _transSummaryService = transSummaryService ?? throw new ArgumentNullException(nameof(transSummaryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// Get all transactions
        /// </summary>
        /// <response code="200">Return a list of transactions</response>
        /// <response code="404">Not fount transactions</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions(CancellationToken cancellationToken)
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionsAsync(cancellationToken);
                if (transactions == null) return NotFound();
                return Ok(transactions);
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

        /// <summary>
        /// Get all transactions by Sku
        /// </summary>
        /// <response code="200">Return a object with a list of transactions by sku and total in EUR</response>
        /// <response code="404">Not fount transactions</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("total-with-transactions")]
        public async Task<IActionResult> GetTransactionsBySku([FromQuery] string sku, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(sku))
                throw new ArgumentNullException($"{nameof(sku)} can not be null or empty");

            try
            {
                var transactions = await _transSummaryService.GetTransactionsWithTotal(sku, cancellationToken);
                if (transactions.Total == 0) return NotFound();
                return Ok(transactions);
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
