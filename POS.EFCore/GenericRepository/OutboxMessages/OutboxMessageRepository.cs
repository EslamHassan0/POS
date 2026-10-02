using Microsoft.EntityFrameworkCore;
using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.OutboxMessages;
using POS.Domain.Shared.OutboxMessages;
using POS.EFCore.POSContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.EFCore.GenericRepository.OutboxMessages;

public class OutboxMessageRepository : GenericRepository<OutboxMessage, Guid>, IOutboxMessageRepository, IRepositoryMarker
{
    public OutboxMessageRepository(POSContext context) : base(context)
    {
    }

    public async Task<List<OutboxMessage>> GetPendingMessagesAsync(DateTime availableBefore, int batchSize)
    {
        var _dbContext = await GetDbContextAsync();
        var outbox = await _dbContext.OutboxMessages
            .Where(a=>a.Status == OutboxStatus.Pending && a.AvailableAt <= availableBefore)
            .OrderBy(a=>a.CreatedAt)
            .Take(batchSize)
            .ToListAsync();

        return outbox;
    }
    public async Task<List<OutboxMessage>> GetStuckProcessingMessagesAsync(DateTime lockedBefore)
    {
        var _dbContext = await GetDbContextAsync();
        return await _dbContext.OutboxMessages
            .Where(m => m.Status == OutboxStatus.Processing && m.LockedAt != null && m.LockedAt < lockedBefore)
            .ToListAsync();
    }
    public async Task<List<OutboxMessage>> GetCompletedMessagesOlderThanAsync(DateTime olderThen)
    {
        var _dbContext = await GetDbContextAsync();
        var oldMessage = await _dbContext.OutboxMessages
                        .Where(m => m.Status == OutboxStatus.Completed && m.CreatedAt < olderThen)
                        .ToListAsync();
        
        return oldMessage;
    }
}
