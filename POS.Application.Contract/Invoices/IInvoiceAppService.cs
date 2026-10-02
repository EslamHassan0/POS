using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Invoices
{
    public interface IInvoiceAppService : IApplicationService
    {
        Task<Result<InvoiceDto>> CreateAsync(CreateInvoiceDto invoiceDto);

        Task<PagedResultDTo<InvoiceWithDetailsDto>> GetListAsync(InvoiceInput input);

        Task<InvoiceDto> GetAsync(Guid id);

        Task<Result<InvoiceDto>> CancelAsync(Guid invoiceId);

        Task<Result<InvoiceDto>> PayAsync(Guid invoiceId);

        Task<InvoiceDto> GetWithDetailsAsync(Guid id);
    }
}
