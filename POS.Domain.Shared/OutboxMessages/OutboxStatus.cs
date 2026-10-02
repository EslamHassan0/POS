using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Shared.OutboxMessages;

public enum OutboxStatus
{
    Pending = 0,

    Queued = 1,

    Processing = 2,

    Completed = 3,


    DeadLetter = 4,
    Failed = 5,
}
