using POS.Domain.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.BackgroundJobs;

public static class RetryPolicy
{
    public const int MaxAttempts = 5;
    private static readonly TimeSpan[] Backoffs =
    {
        TimeSpan.FromMinutes(2),
        TimeSpan.FromMinutes(5),
        TimeSpan.FromMinutes(15),
        TimeSpan.FromHours(1)
    };
    public static TimeSpan GetNextDelay(int attemptsNumber)
    {
        var index = Math.Min(attemptsNumber-1,Backoffs.Length-1);
        return Backoffs[index];
    }
    public static bool IsRetryable(Exception ex)
    {
        return ex switch
        {
            NotFoundException => false,
            _ => true
        };
    }
}
