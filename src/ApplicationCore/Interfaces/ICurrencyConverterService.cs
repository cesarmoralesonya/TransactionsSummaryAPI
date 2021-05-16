using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICurrencyConverterService
    {
        public Task Initialization { get; }
        public decimal ExchangeRate(string baseCode, string targetCode);
    }
}
