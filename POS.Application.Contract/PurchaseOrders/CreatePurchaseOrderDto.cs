using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.PurchaseOrders
{
    public class CreatePurchaseOrderDto
    {
        public DateTime Date { get; set; }
        public Guid SupplierId { get; set; }
        public Guid WarehouseId { get; set; }
        public List<CreatePurchaseOrderItemDto> Items { get; set; }
    }

    public class CreatePurchaseOrderItemDto
    {
        public Guid ItemId { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
