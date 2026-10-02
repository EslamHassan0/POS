using Hangfire;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POS.Domain.Entity.OutboxMessages;
using POS.Domain.Shared.OutboxMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.BackgroundJobs;

public class OutboxDispatcher : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public OutboxDispatcher(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) 
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();


            var stuckMessages = await outboxRepository.GetStuckProcessingMessagesAsync(
                lockedBefore: DateTime.UtcNow.AddMinutes(-5));

            foreach (var stuck in stuckMessages)
            {
                stuck.Status = OutboxStatus.Pending;
                stuck.LockedAt = null;
                await outboxRepository.UpdateAsync(stuck);
            }

            var messages =
                await outboxRepository
                .GetPendingMessagesAsync(DateTime.UtcNow, 50);

            foreach (var message in messages)
            {
                BackgroundJob.Enqueue<ProcessOutboxJob>(x => x.Process(message.Id));
                message.Status = OutboxStatus.Queued;
                await outboxRepository.UpdateAsync(message);
            }


            await Task.Delay(TimeSpan.FromSeconds(5),stoppingToken);
        }   
    }
}
