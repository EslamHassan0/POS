using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.InvoiceItems
{
    public class UpdateInvoiceItemDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public decimal Quantity { get; set; }
    }
}
