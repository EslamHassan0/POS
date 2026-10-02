using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using POS.Domain.GenericRepository;
using POS.Domain.Shared.Common;
using POS.Domain.Shared.Exceptions;
using POS.EFCore.POSContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POS.EFCore.GenericRepository
{
    public class GenericRepository<T , IKey> : IGenericRepository<T, IKey> where T : class
    {
        private readonly POSContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(POSContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(IKey id,bool noTraking = false)
        {
            var query = _dbSet.AsQueryable();
            if (noTraking)
            {
                query = query.AsNoTracking();
            }

            var entity = await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id").Equals(id));
            if (entity is null)
                throw new KeyNotFoundException($"The requested resource was not found. Id {id}");

            return entity; 
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex) when (entity is IConcurrentEntity)
            {
                var entry = ex.Entries.Single();
                var dbValues = await entry.GetDatabaseValuesAsync();

                if (dbValues == null)
                    throw new NotFoundException("The record was deleted from another source.");

                throw new ConcurrencyConflictException("The record was edited by someone else before you.");
            }

            return entity;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(List<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<TResult>> GetProjectedAsync<TResult>(Expression<Func<T, TResult>> selector)
        {
            return await _dbSet.Select(selector).ToListAsync();
        }


        public IQueryable<T> GetQueryable()
            => _context.Set<T>();
 

        public IQueryable<T> GetDbSet()
        {
            return _dbSet;
        }
        public async Task<POSContext> GetDbContextAsync()
        {
            return await Task.FromResult(_context);
        }
        public async Task<T?> FindAsync(IKey id)
        {
            return await _dbSet.FindAsync(id);
        }
        public IQueryable<T> WithDetails(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }
        
    }

}
