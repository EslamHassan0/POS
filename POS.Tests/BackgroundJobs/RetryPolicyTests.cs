
using FluentAssertions;
using POS.Application.BackgroundJobs;
using POS.Domain.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Tests.BackgroundJobs;

public class RetryPolicyTests
{
    [Theory]
    [InlineData(1, 2)]   
    [InlineData(2, 5)]   
    [InlineData(3, 15)]  
    [InlineData(4, 60)]  
    public void GetNextDelay_ReturnsCorrectBackoff_ForEachAttempt(int attemptNumber, int expectedMinutes)
    {
        var delay = RetryPolicy.GetNextDelay(attemptNumber);

        delay.Should().Be(TimeSpan.FromMinutes(expectedMinutes));
    }
    [Fact]
    public void GetNextDelay_AttemptBeyondMax_StaysAtLastBackoff()
    {
        var delay = RetryPolicy.GetNextDelay(10);

        delay.Should().Be(TimeSpan.FromHours(1));
    }

    [Fact]
    public void GetNextDelay_FirstAttempt_IsShortestDelay()
    {
        var delay = RetryPolicy.GetNextDelay(1);

        delay.Should().Be(TimeSpan.FromMinutes(2));
    }
    // ---------- IsRetryable ----------

    [Fact]
    public void IsRetryable_InvoiceNotFoundException_ReturnsFalse()
    {
        var ex = new NotFoundException(Guid.NewGuid().ToString());

        var result = RetryPolicy.IsRetryable(ex);

        result.Should().BeFalse();
    }

    [Fact]
    public void IsRetryable_GenericException_ReturnsTrue()
    {
        var ex = new Exception("some transient error");

        var result = RetryPolicy.IsRetryable(ex);

        result.Should().BeTrue();
    }

    [Fact]
    public void IsRetryable_IOException_ReturnsTrue()
    {
        var ex = new System.IO.IOException("disk full");

        var result = RetryPolicy.IsRetryable(ex);

        result.Should().BeTrue();
    }


    // ---------- MaxAttempts ----------
    [Fact]
    public void MaxAttempts_IsFive()
    {
        RetryPolicy.MaxAttempts.Should().Be(5);
    }

}
