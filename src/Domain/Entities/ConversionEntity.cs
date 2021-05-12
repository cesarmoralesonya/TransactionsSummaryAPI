using Domain.Interfaces;
using System;

namespace Domain.Entities
{
    public class ConversionEntity : EntityBase, IAgragateRoot
    {
        public string From { get; private set; }

        public string To { get; private set; }

        public double Rate { get; private set; }

        public ConversionEntity()
        {
            //Required by EF
        }

        public ConversionEntity(string from, string to, double rate)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
            Rate = rate;
        }
    }
}
