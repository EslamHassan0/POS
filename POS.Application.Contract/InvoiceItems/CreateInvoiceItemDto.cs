using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.InvoiceItems
{
    public class CreateInvoiceItemDto
    {
        public Guid ItemId { get; set; }
        public decimal Qty { get; set; }
    }
}
