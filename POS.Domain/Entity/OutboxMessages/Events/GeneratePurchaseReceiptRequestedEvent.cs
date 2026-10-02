using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.OutboxMessages.Events;

public class GeneratePurchaseReceiptRequestedEvent
{
    public Guid PurchaseOrderId { get; set; }
}
