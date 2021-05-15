using Domain.Interfaces;
using System;

namespace Domain.Entities
{
    public class RateEntity : EntityBase, IAgragateRoot
    {
        public string From { get; private set; }

        public string To { get; private set; }

        public decimal Rate { get; private set; }

        public RateEntity()
        {
            //Required by EF
        }

        public RateEntity(string from, string to, decimal rate)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
            Rate = rate;
        }
    }
}
