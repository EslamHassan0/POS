using POS.Domain.BaseEntity;
using POS.Domain.Shared.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.PaymentMethods
{
    public class PaymentMethod : BaseEntity<Guid>
    {
        public PaymentType Name { get; set; } // Cash, Visa, Mastercard
        public bool IsActive { get; set; }
    }

}
