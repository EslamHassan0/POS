using POS.Domain.BaseEntity;
using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.PaymentMethods;
using POS.Domain.Shared.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.Payments
{
    public class Payment : BaseEntity<Guid>
    {
        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public Guid PaymentMethodId { get; set; }       // ← بدل PaymentType
        public PaymentMethod PaymentMethod { get; set; } // العلاقة
        public decimal Amount { get; set; }
    }
}
