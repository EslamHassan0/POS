using POS.Domain.Entity.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.PurchaseOrders.PurchaseOrderItems;

public class PurchaseOrderItemWithDetails
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; }
    public decimal Qty { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalLine { get; set; }

    public Guid PurchaseOrderId { get; set; }
 

}
