using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Domain.Entity.Invoices;
using POS.Domain.GenericRepository;
using POS.Domain.Shared.PurchaseOrderCheckTypes;

namespace POS.Domain.Entity.PurchaseOrders
{
    public interface IPurchaseOrderRepository : IGenericRepository<PurchaseOrder,Guid>
    {
        Task<string> GetNextOrderNo();


        Task<List<PurchaseOrderWithDetails>> GetListAsync(
        string orderNo = null,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            string orderNo = null,
            CancellationToken cancellationToken = default);

        Task<PurchaseOrder> GetPurchaseOrderWithDetailsById(Guid id);



        Task<bool> HasPurchaseOrderId(Guid id, PurchaseOrderCheckType checkType);

        Task<PurchaseOrderWithDetails> GetAsync(Guid id);
    }
}
