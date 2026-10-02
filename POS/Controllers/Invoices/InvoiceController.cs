using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Attributes;
using POS.Application.Contract.Invoices;
using POS.Application.Contract.Permissions;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;

namespace POS.API.Controllers.Invoices
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase , IInvoiceAppService
    {
        private readonly IInvoiceAppService _invoiceAppService;

        public InvoiceController(IInvoiceAppService invoiceAppService)
        {
            _invoiceAppService = invoiceAppService;
        }
        [HttpGet("Cancel/{invoiceId}")]
        public Task<Result<InvoiceDto>> CancelAsync(Guid invoiceId)
        {
            return _invoiceAppService.CancelAsync(invoiceId);
        }
        [HttpPost("Pay/{invoiceId}")]
        [Idempotent(60)]
        [Authorize(Policy = POSPermissions.Sales.Pay)]
        public Task<Result<InvoiceDto>> PayAsync(Guid invoiceId)
        {
            return _invoiceAppService.PayAsync(invoiceId);
        }

        [HttpPost]
        [Idempotent(60)]
        [Authorize(Policy = POSPermissions.Sales.Create)]
        public Task<Result<InvoiceDto>> CreateAsync(CreateInvoiceDto invoiceDto)
        {
           return _invoiceAppService.CreateAsync(invoiceDto);
        }
        [HttpGet("{id}")]
        public Task<InvoiceDto> GetAsync(Guid id)
        {
            return _invoiceAppService.GetAsync(id);
        }
        [HttpGet]
        public Task<PagedResultDTo<InvoiceWithDetailsDto>> GetListAsync([FromQuery] InvoiceInput input)
        {
            return _invoiceAppService.GetListAsync(input);
        }
        [HttpGet("GetWithDetails/{id}")]
        public Task<InvoiceDto> GetWithDetailsAsync(Guid id)
        {
            return _invoiceAppService.GetWithDetailsAsync(id);
        }

        [HttpGet("{invoiceId}/receipt")]
        public async Task<IActionResult> GetReceipt(Guid invoiceId)
        {
            var invoice = await _invoiceAppService.GetAsync(invoiceId);

            if (invoice?.ReceiptPath is null || !System.IO.File.Exists(invoice.ReceiptPath))
                return NotFound("Receipt not generated yet");

            var bytes = await System.IO.File.ReadAllBytesAsync(invoice.ReceiptPath);

            return File(bytes, "application/pdf", $"receipt-{invoice.InvoiceNo}.pdf");
        }
    }
}
