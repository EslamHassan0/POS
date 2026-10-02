using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Invoices
{
    public class FinalPerLinePriceDto
    {
        public decimal TotalBeforeTax { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAfterTax { get; set; }
    }
}
