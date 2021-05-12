using Domain.Interfaces;
using Infraestructure.Interfaces;

namespace Infraestructure.Models
{
    public class ConversionModel : IWebServiceModel
    {
        public string From { get; set; }

        public string To { get; set; }

        public double Rate { get; set; }
    }
}
