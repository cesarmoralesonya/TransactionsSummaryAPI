using ApplicationCore.Interfaces;
using System;

namespace ApplicationCore.Entities
{
    public class ConversionDb : BaseEntity, IAgragateRoot
    {
        public string From { get; private set; }

        public string To { get; private set; }

        public decimal Rate { get; private set; }

        public ConversionDb()
        {
            //Required by EF
        }

        public ConversionDb(string from, string to, decimal rate)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
            Rate = rate;
        }
    }
}
