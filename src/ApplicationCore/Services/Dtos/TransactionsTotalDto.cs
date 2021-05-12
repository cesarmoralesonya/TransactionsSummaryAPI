using System.Collections.Generic;

namespace ApplicationCore.Services.Dtos
{
    public class TransactionsTotalDto
    {
        public List<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();

        public double Total { get; set; }
    }
}
