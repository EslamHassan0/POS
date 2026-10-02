using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Receipts;

public interface IReceiptService : IApplicationService
{
    Task<string> GenerateInvoiceAsync( Guid invoiceId);
    Task<string> GeneratePurchaseReceiptAsync( Guid purchaseOrderId);
}
