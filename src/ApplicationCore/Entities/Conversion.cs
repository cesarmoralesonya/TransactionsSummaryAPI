using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class Conversion: IWebServicesEntity
    {
        public string From { get; set; }

        public string To { get; set; }

        public decimal Rate { get; set; }
    }
}
