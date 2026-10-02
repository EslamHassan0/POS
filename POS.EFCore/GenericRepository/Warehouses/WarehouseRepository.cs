using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entity.Customers;
using POS.Domain.Entity.Suppliers;
using POS.Domain.Entity.Warehouses;
using POS.Domain.Shared.Extensions;
using POS.Domain.Shared.SharedLookupDtos;
using POS.EFCore.POSContexts;

namespace POS.EFCore.GenericRepository.Warehouses
{
    public class WarehouseRepository : GenericRepository<Warehouse, Guid>, IWarehouseRepository, IRepositoryMarker
    {
        public WarehouseRepository(POSContext context) : base(context)
        {
        }

        public async Task<long> GetCountAsync(string filterText = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query = await ApplyFilter(filterText);
            return await query.LongCountAsync(cancellationToken);
        }

        public async Task<List<Warehouse>> GetListAsync(string filterText = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query = await ApplyFilter(filterText);
            //query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }
        protected async Task<IQueryable<Warehouse>> ApplyFilter(string filterText = null)
        {
            var _dbContext = await GetDbContextAsync();
            var query = (from warehouse in _dbContext.Warehouses
                         where filterText == null ||
                         warehouse.Name.ToLower().Contains(filterText.ToLower()) ||
                         warehouse.Location.ToLower().Contains(filterText.ToLower())
                         select warehouse);
                          


            return query;
        }

        public async Task<List<SharedLookUp>> GetWarehouseLookupAsync()
        {
            var _dbContext = await GetDbContextAsync();
            var items = _dbContext.Warehouses
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
