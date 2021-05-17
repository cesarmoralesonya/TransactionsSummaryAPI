using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICurrencyConverterService : IAsyncInitialization
    {
        public decimal ExchangeRate(string baseCode, string targetCode);
    }
}
