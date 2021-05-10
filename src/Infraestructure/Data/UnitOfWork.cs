using ApplicationCore.Interfaces;
using Infraestructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TransSummaryContext _context;
        private ConversionRepository _conversionRepository;
        private TransactionRepository _transactionRepository;

        public UnitOfWork(TransSummaryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IConversionRepository Conversions => _conversionRepository = _conversionRepository ?? new ConversionRepository(_context);

        public ITransactionRepository Transactions => _transactionRepository = _transactionRepository ?? new TransactionRepository(_context);

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
