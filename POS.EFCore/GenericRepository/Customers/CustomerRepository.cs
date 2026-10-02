using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entity.Customers;
using POS.Domain.Entity.Invoices;
using POS.Domain.Shared.Customers;
using POS.Domain.Shared.Extensions;
using POS.Domain.Shared.Invoices;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.SharedLookupDtos;
using POS.EFCore.POSContexts;

namespace POS.EFCore.GenericRepository.Customers
{
    public class CustomerRepository : GenericRepository<Customer, Guid>, ICustomerRepository , IRepositoryMarker
    {
        public CustomerRepository(POSContext context) : base(context)
        {
        }

        public async Task<List<Customer>> GetListAsync(string? filterText = null, string? customerName = null ,  string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter(GetQueryable(),filterText, customerName);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CustomerConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected  IQueryable<Customer> ApplyFilter(IQueryable<Customer> query ,string? filterText = null, string? customerName = null)
        {
            query = query
                            .WhereIf(!string.IsNullOrEmpty(filterText), a => a.Name.ToLower().Contains(filterText.ToLower()))
                            .WhereIf(!string.IsNullOrEmpty(customerName), a => a.Name.ToLower().Contains(customerName.ToLower()));

            return query;
        }

        public async Task<List<SharedLookUp>> GetCustomerLookupAsync()
        {
            var _dbContext = await GetDbContextAsync();
            var items = _dbContext.Customers
                        .AsNoTracking()
                        .Select(a => new SharedLookUp
                        {
                            Id = a.Id,
                            Name = a.Name
                        })
                        .ToList();

            return items;
        }
    }
}
