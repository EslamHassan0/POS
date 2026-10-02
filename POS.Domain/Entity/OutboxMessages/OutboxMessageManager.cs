using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.OutboxMessages.Events;
using POS.Domain.Events;
using POS.Domain.GenericRepository;
using POS.Domain.Shared.OutboxMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace POS.Domain.Entity.OutboxMessages;

public class OutboxMessageManager : DomainServices
{
    private readonly IGenericRepository<OutboxMessage, Guid> _outboxRepository;

    public OutboxMessageManager(IGenericRepository<OutboxMessage, Guid> outboxRepository)
    {
        _outboxRepository = outboxRepository;
    }

    public async Task<OutboxMessage> CreateMessageAsync<T>(Guid aggregateId,
        OutboxMessageType type,
        T payload)
    {
        var outbox = new OutboxMessage
        {
            Id = Guid.NewGuid(),

            Type = type,

            AggregateId = aggregateId,

            Payload = JsonSerializer.Serialize(payload),

            Status = OutboxStatus.Pending,

            Attempts = 0,

            CreatedAt = DateTime.UtcNow,

            AvailableAt = DateTime.UtcNow
        };

        return await _outboxRepository.AddAsync(outbox);
    }
}
