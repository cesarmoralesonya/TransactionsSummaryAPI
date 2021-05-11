using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.Controllers
{
    [Produces("application/json")]
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController: ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTransactionsBySku([FromQuery]string sku, CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
