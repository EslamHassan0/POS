using Microsoft.EntityFrameworkCore.Storage;
using POS.Contract.IUnitOfWorks;
using POS.EFCore.POSContexts;
namespace POS.EFCore.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly POSContext _context;
        private  IDbContextTransaction _transaction;

        public UnitOfWork(POSContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            await BeginTransactionAsync();
            try
            {
                await action();
                await CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }
    }
}
