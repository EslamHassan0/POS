using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Shared.PurchaseOrderStatus
{
    public enum Status
    {
        Draft,
        Approved,
        Pending,
        Received,
        Cancelled
    }
}
