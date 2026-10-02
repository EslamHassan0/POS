using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Shared.Common;

public interface IAsyncQueryableExecuter
{
    Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken ct = default);
    Task<decimal> SumAsync<T>(IQueryable<T> query, Expression<Func<T, decimal>> selector, CancellationToken ct = default);
    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken ct = default);
    Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken ct = default);
}
