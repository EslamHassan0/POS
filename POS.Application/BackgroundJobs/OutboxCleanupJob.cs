using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Domain.Entity.OutboxMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.BackgroundJobs;

public class OutboxCleanupJob
{
    private readonly IOutboxMessageRepository _outboxRepository;
    private readonly ILogger<OutboxCleanupJob> _logger;

    private const int RetentionDays = 30;

    public OutboxCleanupJob(
        IOutboxMessageRepository outboxRepository,
        ILogger<OutboxCleanupJob> logger)
    {
        _outboxRepository = outboxRepository;
        _logger = logger;
    }
    public async Task Run()
    {
        var cutoff = DateTime.UtcNow.AddDays(-RetentionDays);

        var outBox = await _outboxRepository.GetCompletedMessagesOlderThanAsync(cutoff);

        if (!outBox.Any())
            return;

         _outboxRepository.DeleteRange(outBox);
         await _outboxRepository.SaveChangesAsync();

        _logger.LogInformation(
            "Outbox cleanup: deleted {Count} completed messages older than {Cutoff}",
            outBox.Count, cutoff);
    }
}
