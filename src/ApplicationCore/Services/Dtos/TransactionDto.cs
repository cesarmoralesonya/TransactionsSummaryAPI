using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Services.Dtos
{
    public class TransactionDto
    {
        public string Sku { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }
    }
}
