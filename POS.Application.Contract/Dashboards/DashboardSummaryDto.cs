using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Dashboards;

public class DashboardSummaryDto
{
    public decimal TotalSales { get; set; }
    public int InvoicesCount { get; set; }
    public decimal AverageInvoice { get; set; }
    public decimal TotalVat { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalPaymentFees { get; set; }
    public decimal EstimatedProfit { get; set; }
    public int PendingPurchaseOrders { get; set; }
}