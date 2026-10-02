using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using POS.Domain.Entity.Categories;
using POS.Domain.Entity.Items;
using POS.Domain.Shared.Item;
using POS.EFCore.POSContexts;
using POS.Tests.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace POS.Tests.Repositories;

public class ItemConcurrencyTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _output;
    public ItemConcurrencyTests(DatabaseFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }

    [Fact]
    public async Task Update_ShouldThrowConcurrencyConflict_WhenRowChangedByAnotherContext()
    {
         
        using var context1 = new POSContext(_fixture.Options);
        using var context2 = new POSContext(_fixture.Options);
        var categoryId = Guid.NewGuid();
        context1.Categories.Add(new Category(categoryId, "Test Category","sss"));
        await context1.SaveChangesAsync();


        //var itemId = Guid.NewGuid();
        //var categoryId= Guid.NewGuid();
        var item = new Item(Guid.NewGuid(), "Name", "C1", "B1", 10, 5, 0, false, true, categoryId, null, ItemUnit.Piece);
        context1.Items.Add(item);
        await context1.SaveChangesAsync();

        var item1 = await context1.Items.FindAsync(item.Id);
        var item2 = await context2.Items.FindAsync(item.Id);

        item1!.Price = 200;
        await context1.SaveChangesAsync();

        item2!.Price = 300;


      var exception=  await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => context2.SaveChangesAsync());
        _output.WriteLine($"Success: Caught expected concurrency exception: '{exception.Message}'");
        Assert.Contains("modified by another user", exception.Message.ToLower());

    }


    [Fact]
    public async Task Update_ShouldSucceed_WhenNoConflict()
    {
          
        using var context = new POSContext(_fixture.Options);
        var categoryId = Guid.NewGuid();
        context.Categories.Add(new Category(categoryId, "Test Category","sss"));
        await context.SaveChangesAsync();


        var item = new Item(Guid.NewGuid(), "Name", "C1", "B1", 10, 5, 0, false, true, categoryId, null, ItemUnit.Piece);
        context.Items.Add(item);
        await context.SaveChangesAsync();

        // الآن item.RowVersion يحتوي على القيمة الحقيقية التي ولدها SQL Server
        item.Price = 999;
        await context.SaveChangesAsync(); // سيعمل لأن الـ RowVersion متطابق

        using var verifyContext = new POSContext(_fixture.Options);
        var updated = await verifyContext.Items.FindAsync(item.Id);
        Assert.Equal(999, updated!.Price);
    }
}
