using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IConversionRepository Conversions { get; }
        ITransactionRepository Transactions { get; }

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
