using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.GenericRepository
{
    public interface IGenericRepository<TEntity, IKey> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(IKey id, bool noTraking = false);
        Task<TEntity?> FindAsync(IKey id);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        void Delete(TEntity entity);
        void DeleteRange(List<TEntity> entity);
        Task SaveChangesAsync();
        IQueryable<TEntity> GetQueryable();
        IQueryable<TEntity> GetDbSet();
        //Task<DbContext> GetDbContextAsync();
        IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] includes);
        Task<List<TResult>> GetProjectedAsync<TResult>(Expression<Func<TEntity, TResult>> selector);
    }
}
