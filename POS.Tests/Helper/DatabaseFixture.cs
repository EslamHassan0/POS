using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using POS.EFCore.POSContexts;
using SkiaSharp;

namespace POS.Tests.Helper;

public class DatabaseFixture : IAsyncLifetime
{
    public DbContextOptions<POSContext> Options { get; private set; }

    public async Task InitializeAsync()
    {
         
        var connectionString = "Server=(localdb)\\mssqllocaldb;Database=TestDb_Shared;Trusted_Connection=True;";

        Options = new DbContextOptionsBuilder<POSContext>()
            .UseSqlServer(connectionString)
            .Options;

        using var context = new POSContext(Options);
        await context.Database.EnsureDeletedAsync();  
        await context.Database.EnsureCreatedAsync();  
    }

    public async Task DisposeAsync()
    {
        using var context = new POSContext(Options);
        await context.Database.EnsureDeletedAsync(); 
    }
}
