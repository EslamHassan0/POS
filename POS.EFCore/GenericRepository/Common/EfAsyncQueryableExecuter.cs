using Microsoft.EntityFrameworkCore;
using POS.Domain.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POS.EFCore.GenericRepository.Common;

public class EfAsyncQueryableExecuter : IAsyncQueryableExecuter
{
    public Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken ct = default)
        => query.CountAsync(ct);

    public Task<decimal> SumAsync<T>(IQueryable<T> query, Expression<Func<T, decimal>> selector, CancellationToken ct = default)
        => query.SumAsync(selector, ct);

    public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken ct = default)
        => query.FirstOrDefaultAsync(ct);

    public Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken ct = default)
        => query.ToListAsync(ct);
}
