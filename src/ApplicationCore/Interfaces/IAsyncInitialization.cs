using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAsyncInitialization
    {
        public Task Initialization { get; }
    }
}
