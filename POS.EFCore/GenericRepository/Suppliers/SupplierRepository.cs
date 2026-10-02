using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entity.Items;
using POS.Domain.Entity.PurchaseOrders;
using POS.Domain.Shared.Extensions;
using POS.Domain.Shared.Item;
using POS.Domain.Shared.SharedLookupDtos;
using POS.EFCore.GenericRepository;
using POS.EFCore.POSContexts;

namespace POS.Domain.Entity.Suppliers
{
    public class SupplierRepository : GenericRepository<Supplier, Guid>, ISupplierRepository, IRepositoryMarker
    {
        public SupplierRepository(POSContext context) : base(context)
        {

        }

        public async Task<long> GetCountAsync(string filterText = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query = await ApplyFilter(filterText);
            return await query.LongCountAsync(cancellationToken);
        }

        public async Task<List<SupplierWithOrders>> GetListAsync(string filterText = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query =await  ApplyFilter(filterText);
            //query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }
        protected async Task<IQueryable<SupplierWithOrders>> ApplyFilter(string filterText = null)
        {
            var _dbContext = await GetDbContextAsync();
            var query = (from supplier in _dbContext.Suppliers
                         where filterText == null || supplier.Name.ToLower().Contains(filterText.ToLower())
                         select new SupplierWithOrders
                         {
                             Id = supplier.Id,
                             Name = supplier.Name,
                             Phone = supplier.Phone,
                             Email = supplier.Email,
                             Address = supplier.Address,
                             OrdersCount = _dbContext.PurchaseOrders
                                .Count(p => p.SupplierId == supplier.Id)
                         });
            return query;
        }

        public async Task<List<SharedLookUp>> GetSupplierLookupAsync()
        {
            var _dbContext = await GetDbContextAsync();
            var items = _dbContext.Suppliers
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
