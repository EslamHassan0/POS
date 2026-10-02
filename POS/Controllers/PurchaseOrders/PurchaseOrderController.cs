using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Contract.PurchaseOrders;
using POS.Application.PurchaseOrders;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;

namespace POS.API.Controllers.PurchaseOrders
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase, IPurchaseOrderAppService
    {
        private readonly IPurchaseOrderAppService _purchaseOrderAppService;

        public PurchaseOrderController(IPurchaseOrderAppService purchaseOrderAppService)
        {
            _purchaseOrderAppService = purchaseOrderAppService;
        }
        [HttpPut("Cancel/{orderId}")]
        public Task<Result<PurchaseOrderDto>> CancelAsync(Guid orderId)
        {
            return _purchaseOrderAppService.CancelAsync(orderId);
        }
        [HttpPost]
        public Task<Result<PurchaseOrderDto>> CreateAsync(CreatePurchaseOrderDto dto)
        {
            return _purchaseOrderAppService.CreateAsync(dto);
        }
        [HttpGet("{id}")]
        public Task<PurchaseOrderWithDetailsDto> GetAsync(Guid id)
        {
            return _purchaseOrderAppService.GetAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDTo<PurchaseOrderDto>> GetListAsync([FromQuery]PurchaseOrderInput input)
        {
            return _purchaseOrderAppService.GetListAsync(input);
        }

        [HttpPut("Receive/{orderId}")]
        public Task<Result<PurchaseOrderDto>> ReceiveAsync(Guid orderId)
        {
            return _purchaseOrderAppService.ReceiveAsync(orderId);
        }
        [HttpGet("{invoiceId}/receipt")]
        public async Task<IActionResult> GetReceipt(Guid invoiceId)
        {
            var invoice = await _purchaseOrderAppService.GetAsync(invoiceId);

            if (invoice?.ReceiptPath is null || !System.IO.File.Exists(invoice.ReceiptPath))
                return NotFound("Receipt not generated yet");

            var bytes = await System.IO.File.ReadAllBytesAsync(invoice.ReceiptPath);

            return File(bytes, "application/pdf", $"receipt-{invoice.PurchaseOrderNo}.pdf");
        }
    }
}
