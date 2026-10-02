using Microsoft.Extensions.Logging;
using Moq;
using POS.Application.BackgroundJobs;
using POS.Domain.Entity.OutboxMessages;
using POS.Domain.Shared.OutboxMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Tests.BackgroundJobs;

public class OutboxCleanupJobTests
{
    [Fact]
    public async Task Run_CallsRepostioryWithCorrectCutoffData()
    {
        //Arrange
        var repositoryMock = new Mock<IOutboxMessageRepository>();
        var loggerMock = new Mock<ILogger<OutboxCleanupJob>>();


        var oldMessages = new List<OutboxMessage>
        {
            new OutboxMessage { Id = Guid.NewGuid(), Status = OutboxStatus.Completed },
            new OutboxMessage { Id = Guid.NewGuid(), Status = OutboxStatus.Completed }
        };

        repositoryMock
            .Setup(r => r.GetCompletedMessagesOlderThanAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(oldMessages);

        var job = new OutboxCleanupJob(repositoryMock.Object, loggerMock.Object);
        await job.Run();


        repositoryMock.Verify(r => r.GetCompletedMessagesOlderThanAsync(
          It.Is<DateTime>(d => d <= DateTime.UtcNow.AddDays(-29))),
          Times.Once);

        repositoryMock.Verify(r => r.DeleteRange(oldMessages), Times.Once);
        repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
    
}
