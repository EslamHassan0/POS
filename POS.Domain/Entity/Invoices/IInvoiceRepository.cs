using POS.Domain.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.Invoices
{
    public interface IInvoiceRepository : IGenericRepository<Invoice, Guid>
    {
        Task<string> GetNextInvoice();

        Task<List<InvoiceWithDetails>> GetListAsync(
       string filterText = null,
       string invoiceNo = null,
       string sorting = null,
       int maxResultCount = int.MaxValue,
       int skipCount = 0,
       CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            string filterText = null,
            string invoiceNo = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default);
        Task<Invoice> GetWithDetailsAsync(Guid id);
        Task<InvoiceWithDetails> GetInvoiceReport(Guid id);




    }
}
