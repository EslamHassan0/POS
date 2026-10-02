using Microsoft.EntityFrameworkCore;
using POS.Domain.Entity.Customers;
using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.PurchaseOrders.PurchaseOrderItems;
using POS.Domain.Entity.Suppliers;
using POS.Domain.Entity.Warehouses;
using POS.Domain.Shared.Extensions;
using POS.Domain.Shared.Invoices;
using POS.Domain.Shared.PurchaseOrderCheckTypes;
using POS.Domain.Shared.PurchaseOrderStatus;
using POS.EFCore.GenericRepository;
using POS.EFCore.POSContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.PurchaseOrders
{
    public class PurchaseOrderRepository : GenericRepository<PurchaseOrder, Guid>, IPurchaseOrderRepository, IRepositoryMarker
    {
        public PurchaseOrderRepository(POSContext context) : base(context)
        {

        }

        public async Task<long> GetCountAsync(string orderNo = null, CancellationToken cancellationToken = default)
        {
            var query = await ApplyFilter(orderNo);
            return await query.LongCountAsync(cancellationToken);
        }

        public async Task<List<PurchaseOrderWithDetails>> GetListAsync(string orderNo = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query = await ApplyFilter(orderNo);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PurchaseOrderConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }
        protected async Task<IQueryable<PurchaseOrderWithDetails>> ApplyFilter(string orderNo = null)
        {
            var _dbContext = await GetDbContextAsync();

          
            var result  = (from purchaseOrder in _dbContext.PurchaseOrders
                         join supplier in _dbContext.Suppliers on purchaseOrder.SupplierId equals supplier.Id
                          where orderNo == null || supplier.Name.ToLower().Contains(orderNo.ToLower()) || 
                          orderNo == null || purchaseOrder.PurchaseOrderNo.ToLower().Contains(orderNo.ToLower())
                           select new PurchaseOrderWithDetails
                            {
                             Id= purchaseOrder.Id,
                             SupplierId = purchaseOrder.SupplierId,
                             Date = purchaseOrder.Date,
                             PurchaseOrderNo = purchaseOrder.PurchaseOrderNo,
                             Status = purchaseOrder.Status,
                             SupplierName = supplier.Name,
                             TotalAmount = purchaseOrder.TotalAmount,
                             //TotalOrder = 
                         });
            return result;
                
        }
        public async Task<string> GetNextOrderNo()
        {
            var dbSet =  GetDbSet();
            var _dbContext = await GetDbContextAsync();
            var perfixOrder = await _dbContext.POSSettings
                    .FirstOrDefaultAsync();


            var lastOrder = await dbSet
                //.Where(x => x.PurchaseOrderNo.StartsWith("PO-"))
                .OrderByDescending(x => x.PurchaseOrderNo)
                .Select(x => x.PurchaseOrderNo)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (!string.IsNullOrEmpty(lastOrder))
            {
                var numberPart = lastOrder.Replace("PO-", "");
                int.TryParse(numberPart, out nextNumber);
                nextNumber++;
            }

            var perfixPart = perfixOrder.InvoicePrefix ?? "PO";
            return $"{perfixPart}-{nextNumber:D3}";
        }
        public async Task<PurchaseOrder> GetPurchaseOrderWithDetailsById(Guid id)
        {
            var _dbContext = await GetDbContextAsync();
            var order = await _dbContext.PurchaseOrders
                .Include(a => a.PurchaseOrderItems)
                .FirstOrDefaultAsync(a => a.Id == id);
            return order;
        }

       
        public async Task<bool> HasPurchaseOrderId(Guid id , PurchaseOrderCheckType checkType)
        {
            var _dbContext = await GetDbContextAsync();           

         return checkType == PurchaseOrderCheckType.Supplier 
                ? await _dbContext.PurchaseOrders.AsNoTracking().AnyAsync(a => a.SupplierId == id) 
                : await _dbContext.PurchaseOrders.AsNoTracking().AnyAsync(a => a.WarehouseId == id);

          
        }

        public async Task<PurchaseOrderWithDetails> GetAsync(Guid id)
        {
            var _dbContext = await GetDbContextAsync();

            var query = await (from purchaseOrder in _dbContext.PurchaseOrders
                               join supplier in _dbContext.Suppliers on purchaseOrder.SupplierId equals supplier.Id
                               where purchaseOrder.Id == id
                               select new PurchaseOrderWithDetails
                               {
                                   Id = purchaseOrder.Id,
                                   SupplierId = purchaseOrder.SupplierId,
                                   Date = purchaseOrder.Date,
                                   PurchaseOrderNo = purchaseOrder.PurchaseOrderNo,
                                   Status = purchaseOrder.Status,
                                   SupplierName = supplier.Name,
                                   PurchaseOrderItems = _dbContext.PurchaseOrderItems
                                                           .Where(a => a.PurchaseOrderId == purchaseOrder.Id)
                                                           .Select(a => new PurchaseOrderItemWithDetails
                                                           {
                                                               Id = a.Id,
                                                               TotalLine = a.TotalLine,
                                                               UnitPrice = a.UnitPrice,
                                                               Qty = a.Qty,
                                                               ItemName = a.Item.Name,
                                                               
                                                           }).ToList(),
                                   TotalAmount = _dbContext.PurchaseOrderItems
                                                           .Where(a => a.PurchaseOrderId == purchaseOrder.Id)
                                                           .Sum(a => a.TotalLine),
                               })
                      .AsNoTracking()
                      .FirstOrDefaultAsync();

            return query;
        }
    }
}
