using POS.Domain.Entity.Items;
using POS.Domain.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.OutboxMessages;

public interface IOutboxMessageRepository : IGenericRepository<OutboxMessage, Guid>
{
    Task<List<OutboxMessage>> GetPendingMessagesAsync(DateTime availableBefore, int batchSize);

    Task<List<OutboxMessage>> GetStuckProcessingMessagesAsync(DateTime lockedBefore);


    Task<List<OutboxMessage>> GetCompletedMessagesOlderThanAsync(DateTime olderThen);
}
