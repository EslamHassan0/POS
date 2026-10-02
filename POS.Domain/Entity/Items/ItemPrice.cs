using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.Items
{
    public class ItemPrice
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public decimal VatPercent { get; set; }
    }
}
