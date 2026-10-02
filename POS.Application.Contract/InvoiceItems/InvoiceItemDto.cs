using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.InvoiceItems
{
    public class InvoiceItemDto
    {
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal LineTotal { get; set; }
    }
}
