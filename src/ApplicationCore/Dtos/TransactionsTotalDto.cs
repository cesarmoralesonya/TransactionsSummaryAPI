using System.Collections.Generic;

namespace Application.Dtos
{
    public class TransactionsTotalDto
    {
        public List<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();

        public decimal Total { get; set; }
    }
}
