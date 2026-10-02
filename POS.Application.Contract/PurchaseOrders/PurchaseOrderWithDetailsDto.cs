using POS.Domain.Shared.PurchaseOrderStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.PurchaseOrders;

public class PurchaseOrderWithDetailsDto
{
    public Guid Id { get; set; }
    public string PurchaseOrderNo { get; set; }
    public DateTime Date { get; set; }
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; }
    public Status Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string? ReceiptPath { get; set; }
    public DateTime? ReceiptGeneratedAt { get; set; }
    public List<PurchaseOrderItemWithDetailsDto> PurchaseOrderItems { get; set; }
    public int TotalOrder { get; set; }
}
