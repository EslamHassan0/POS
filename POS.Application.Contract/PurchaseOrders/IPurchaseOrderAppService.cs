using POS.Application.Contract;
using POS.Application.Contract.PurchaseOrders;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.PurchaseOrders
{
    public interface IPurchaseOrderAppService : IApplicationService
    {
        Task<Result<PurchaseOrderDto>> CreateAsync(CreatePurchaseOrderDto dto);

        Task<Result<PurchaseOrderDto>> ReceiveAsync(Guid orderId);

        Task<PagedResultDTo<PurchaseOrderDto>> GetListAsync(PurchaseOrderInput input);
        Task<Result<PurchaseOrderDto>> CancelAsync(Guid orderId);

        Task<PurchaseOrderWithDetailsDto> GetAsync(Guid id);
    }
}
