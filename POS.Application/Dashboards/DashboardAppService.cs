using AutoMapper;
using POS.Application.Contract.Dashboards;
using POS.Domain.Entity.InvoiceItems;
using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.PurchaseOrders;
using POS.Domain.GenericRepository;
using POS.Domain.Shared.Common;
using POS.Domain.Shared.Invoices;
using POS.Domain.Shared.PurchaseOrderStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Dashboards;

public class DashboardAppService : ApplicationService, IDashboardAppService
{
    private readonly IGenericRepository<Invoice, Guid> _invoiceRepo;
    private readonly IGenericRepository<InvoiceItem, Guid> _invoiceItemRepo;
    private readonly IGenericRepository<PurchaseOrder, Guid> _purchaseOrderRepo;
    private readonly IAsyncQueryableExecuter _executer;

    public DashboardAppService(IMapper mapper,
        IGenericRepository<Invoice, Guid> invoiceRepo,
        IGenericRepository<InvoiceItem, Guid> invoiceItemRepo,
        IGenericRepository<PurchaseOrder, Guid> purchaseOrderRepo,
        IAsyncQueryableExecuter executer) : base(mapper)
    {
        _invoiceRepo = invoiceRepo;
        _invoiceItemRepo = invoiceItemRepo;
        _purchaseOrderRepo = purchaseOrderRepo;
        _executer = executer;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync(GetDashboardInput input, CancellationToken ct)
    {
        var to = (input.To ?? DateTime.Today).Date;
        var from = (input.From ?? new DateTime(to.Year, to.Month, 1)).Date;
        var toExclusive = to.AddDays(1);

        var invoices = (_invoiceRepo.GetQueryable())
            .Where(i => i.InvoiceDate >= from
            && i.InvoiceDate <= toExclusive
            && i.InvoiceStatus == InvoiceStatus.Paid);

        var summary = await _executer.FirstOrDefaultAsync(
            invoices.GroupBy(_ => 1).Select(g => new DashboardSummaryDto
            {
                TotalSales = g.Sum(x => x.GrandTotal),
                InvoicesCount = g.Count(),
                TotalVat = g.Sum(x => x.VatAmount),
                TotalDiscount = g.Sum(x => x.Discount),
                TotalPaymentFees = g.Sum(x => x.PaymentFee)
            })) ?? new DashboardSummaryDto();

        summary.AverageInvoice = summary.InvoicesCount > 0
            ? Math.Round(summary.TotalSales / summary.InvoicesCount, 2):0;

        var items = _invoiceItemRepo.GetQueryable()
                  .Where(ii => invoices.Any(i => i.Id == ii.InvoiceId));

        summary.EstimatedProfit = await _executer.SumAsync(
            items, ii => (ii.Price - ii.Item.Cost) * ii.Qty);

        summary.PendingPurchaseOrders = await _executer.CountAsync(
            _purchaseOrderRepo.GetQueryable().Where(p => p.Status == Status.Pending));

        return summary;


    }
}
