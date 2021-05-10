using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.Dtos
{
    public class ConversionDto
    {
        public string From { get; set; }

        public string To { get; set; }

        public decimal Rate { get; set; }
    }
}
