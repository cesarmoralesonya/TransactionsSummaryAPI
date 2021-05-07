using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class Conversion: IWebServicesEntity
    {
        public string From { get; private set; }

        public string To { get; private set; }

        public decimal Rate { get; private set; }
    }
}
