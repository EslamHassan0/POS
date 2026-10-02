using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.InvoiceItems;

public class InvoiceItemWithDetails
{
    public Guid ItemId { get; set; }
    public string ItemName { get; set; }
    public decimal Price { get; set; }
    public decimal Qty { get; set; }
    public decimal LineTotal { get; set; }

  
}
