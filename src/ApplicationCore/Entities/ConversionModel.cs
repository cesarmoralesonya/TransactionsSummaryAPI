using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class ConversionModel : IWebServicesEntity
    {
        public string From { get; set; }

        public string To { get; set; }

        public double Rate { get; set; }
    }
}
