using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POS.Application.Contract.Permissions;
using POS.Domain.Entity.Categories;
using POS.Domain.Entity.Customers;
//using POS.Domain.Entity.ItemCategories;
using POS.Domain.Entity.Items;
using POS.Domain.Entity.PaymentMethods;
using POS.Domain.Entity.POSSettings;
using POS.Domain.Shared.Invoices;
using POS.Domain.Shared.Item;
using POS.Domain.Shared.Users;
using POS.EFCore.POSContexts;
using System.Reflection;
using System.Security.Claims;

namespace POS.EFCore.DatabaseSeeders
{

    public class DatabaseSeeder
    {
        private readonly POSContext _context;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public DatabaseSeeder(POSContext context,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }
        public async Task SeedAsync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await SeedPOSSettings();
                await SeedCategoriesAndItems();
                await SeedCustomers();
                await SeedPayments();
                await SeedStock();
                await SeedRolesAsync();

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private async Task SeedRolesAsync()
        {
            await SeedRoleWithPermissionsAsync(RoleConsts.Admin, GetAllPermissions());

           
            await SeedRoleWithPermissionsAsync(RoleConsts.Cashier, new[]
            {
        POSPermissions.Sales.Create,
        POSPermissions.Sales.Edit,
        POSPermissions.Sales.Delete,
        POSPermissions.Sales.Void,
        POSPermissions.Sales.View,
        POSPermissions.Sales.Pay,
        POSPermissions.Sales.Print,
         POSPermissions.Inventory.ViewStock
            });

            
            await SeedRoleWithPermissionsAsync(RoleConsts.Accountant, new[]
            {
        POSPermissions.Purchases.Create,
        POSPermissions.Purchases.Edit,
        POSPermissions.Purchases.Delete,
        POSPermissions.Purchases.View,
        POSPermissions.Purchases.Pay,
        POSPermissions.Purchases.Print,
        POSPermissions.Partners.View,
        POSPermissions.Reports.ViewFinancial,
        POSPermissions.Reports.ViewPurchases,
    });
        }

        private IEnumerable<string> GetAllPermissions()
        {
            return typeof(POSPermissions)
                .GetNestedTypes(BindingFlags.Public | BindingFlags.Static)
                .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                .Where(f => f.IsLiteral && f.FieldType == typeof(string))
                .Select(f => (string)f.GetValue(null));
        }

        private async Task SeedRoleWithPermissionsAsync(string roleName, IEnumerable<string> permissions)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                role = new IdentityRole<Guid>(roleName);
                await _roleManager.CreateAsync(role);
            }

            var existingClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var permission in permissions)
            {
                if (!existingClaims.Any(c => c.Type == "permission" && c.Value == permission))
                {
                    await _roleManager.AddClaimAsync(role, new Claim("permission", permission));
                }
            }
        }

        //private async Task SeedCategories()
        //{
        //    if (!await _context.Set<Category>().AnyAsync())
        //    {
        //        var categories = new List<Category>
        //        {
        //            new Category(Guid.NewGuid(), "Beverages", "Soft drinks, juices, and hot drinks"),
        //            new Category(Guid.NewGuid(), "Snacks", "Chips, biscuits, and light food"),
        //            new Category(Guid.NewGuid(), "Dairy Products", "Milk, cheese, yogurt and dairy items"),
        //            new Category(Guid.NewGuid(), "Frozen Foods", "Frozen meat, vegetables, and ready meals"),
        //            new Category(Guid.NewGuid(), "Bakery", "Bread, cakes, and baked goods"),
        //            new Category(Guid.NewGuid(), "Meat", "Fresh and processed meat products"),
        //            new Category(Guid.NewGuid(), "Vegetables", "Fresh vegetables and greens"),
        //            new Category(Guid.NewGuid(), "Fruits", "Fresh seasonal fruits"),
        //            new Category(Guid.NewGuid(), "Household", "Cleaning and household supplies"),
        //            new Category(Guid.NewGuid(), "Personal Care", "Hygiene and personal care products")
        //        };

        //        _context.Set<Category>().AddRange(categories);
        //        //await _context.SaveChangesAsync();
        //    }
        //}
        private async Task SeedStock()
        {
            //  if (!await _context.Set<StockTransaction>().AnyAsync())
            //  {
            //      if(await _context.Set<Item>().AnyAsync())
            //      {
            //          var items = _itemRepository.GetQueryable();
            //          foreach (var item in items)
            //          {

            //              _context.Set<StockTransaction>().AddRange(
            //                new StockTransaction(
            //                    id: Guid.NewGuid(),
            //                    itemId: item.Id,
            //                    qtyChange: 100,
            //                    transactionType: Domain.Shared.StockTransactions.TransactionType.In,
            //                    referenceId: Guid.NewGuid(),
            //                    date: DateTime.UtcNow
            //                )
            //               );
            //          }
            //      }

            ////await _context.SaveChangesAsync();
            //  }
        }


        //private async Task SeedItems()
        //{
        //    if (!await _context.Set<Item>().AnyAsync())
        //    {
        //        var beverageCategory = await _context.Set<Category>()
        //                                            .FirstAsync(c => c.Name == "Beverages");
        //        var snacksCategory = await _context.Set<Category>()
        //                                           .FirstAsync(c => c.Name == "Snacks");

        //        _context.Set<Item>().AddRange(
        //            new Item(
        //                id: Guid.NewGuid(),
        //                name: "Coca Cola 330ml",
        //                code: "CC330",
        //                barcode: "1234567890123",
        //                price: 10m,
        //                cost: 6m,
        //                vatPercent: 15m,
        //                isVATEligible: true,
        //                isActive: true,
        //                categoryId: beverageCategory.Id,
        //                image: "81zSvcnhEQL._AC_UF894,1000_QL80_.jpg",
        //                ItemUnit.Piece
        //            ),
        //            new Item(
        //                id: Guid.NewGuid(),
        //                name: "Pepsi 330ml",
        //                code: "PEP330",
        //                barcode: "1234567890456",
        //                price: 10m,
        //                cost: 6m,
        //                vatPercent: 15m,
        //                isVATEligible: true,
        //                isActive: true,
        //                categoryId: beverageCategory.Id,
        //                image: "brt00145_01.jpg",
        //                ItemUnit.Piece
        //            ),
        //            new Item(
        //                id: Guid.NewGuid(),
        //                name: "Chips",
        //                code: "CHIPS01",
        //                barcode: "1234567890999",
        //                price: 5m,
        //                cost: 3m,
        //                vatPercent: 15m,
        //                isVATEligible: true,
        //                isActive: true,
        //                categoryId: snacksCategory.Id,
        //                image: "istockphoto-175012912-612x612.jpg",
        //                  ItemUnit.Piece
        //            )
        //        );

        //        //await _context.SaveChangesAsync();
        //    }
        //}

        private async Task SeedCustomers()
        {
            if (!await _context.Set<Customer>().AnyAsync())
            {
                _context.Set<Customer>().AddRange(
                    new Customer { Id = Guid.NewGuid(), Name = "Ahmed Ali", Phone = "01000000001", Address = "Alex" },
                    new Customer { Id = Guid.NewGuid(), Name = "Sara Mohamed", Phone = "01000000002", Address = "Cairo" }
                );
                //await _context.SaveChangesAsync();
            }
        }

        private async Task SeedPayments()
        {
            if (!await _context.Set<PaymentMethod>().AnyAsync())
            {
                _context.Set<PaymentMethod>().AddRange(
                   new PaymentMethod { Id = Guid.NewGuid(), Name = PaymentType.Cash, IsActive = true },
                           new PaymentMethod { Id = Guid.NewGuid(), Name = PaymentType.Visa, IsActive = true }
                );
                //await _context.SaveChangesAsync();
            }
        }

        public async Task SeedCategoriesAndItems()
        {
            if (await _context.Set<Category>().AnyAsync())
                return;

            // ======================
            // 1) CREATE CATEGORIES
            // ======================

            var categories = new List<Category>
            {
                new Category(Guid.NewGuid(), "Beverages", "Cold and hot drinks"),
                new Category(Guid.NewGuid(), "Snacks", "Chips and biscuits"),
                new Category(Guid.NewGuid(), "Dairy", "Milk and cheese"),
                new Category(Guid.NewGuid(), "Bakery", "Bread and pastries"),
                new Category(Guid.NewGuid(), "Meat", "Fresh meat"),
                new Category(Guid.NewGuid(), "Vegetables", "Fresh vegetables"),
                new Category(Guid.NewGuid(), "Fruits", "Fresh fruits"),
                new Category(Guid.NewGuid(), "Household", "Cleaning supplies"),
                new Category(Guid.NewGuid(), "Personal Care", "Hygiene products"),
                new Category(Guid.NewGuid(), "Frozen Foods", "Frozen products")
            };

            await _context.Set<Category>().AddRangeAsync(categories);

            // ======================
            // 2) SAVE FIRST (IMPORTANT)
            // ======================

            await _context.SaveChangesAsync();

            // ======================
            // 3) MAP CATEGORIES
            // ======================

            var beveragesId = categories.First(x => x.Name == "Beverages").Id;
            var snacksId = categories.First(x => x.Name == "Snacks").Id;
            var dairyId = categories.First(x => x.Name == "Dairy").Id;
            var bakeryId = categories.First(x => x.Name == "Bakery").Id;
            var meatId = categories.First(x => x.Name == "Meat").Id;
            var vegId = categories.First(x => x.Name == "Vegetables").Id;
            var fruitsId = categories.First(x => x.Name == "Fruits").Id;
            var houseId = categories.First(x => x.Name == "Household").Id;
            var careId = categories.First(x => x.Name == "Personal Care").Id;
            var frozenId = categories.First(x => x.Name == "Frozen Foods").Id;

            // ======================
            // 4) ITEMS
            // ======================

            var items = new List<Item>
            {
                new Item(Guid.NewGuid(), "Coca Cola 330ml", "BEV-001", "622300123001", 20m, 12m, 14m, true, true, beveragesId, null, ItemUnit.Piece),
                new Item(Guid.NewGuid(), "Pepsi 330ml", "BEV-002", "622300123002", 20m, 12m, 14m, true, true, beveragesId, null, ItemUnit.Piece),
                new Item(Guid.NewGuid(), "Lays Chips", "SNK-001", "622300223001", 15m, 9m, 14m, true, true, snacksId, null, ItemUnit.Piece),
                new Item(Guid.NewGuid(), "Chicken Breast 1kg", "MEA-001", "622300523001", 180m, 140m, 0m, false, true, meatId, null, ItemUnit.Kg)
            };

            await _context.Set<Item>().AddRangeAsync(items);

            await _context.SaveChangesAsync();
        }
        private async Task SeedPOSSettings()
        {
            if (!await _context.Set<POSSetting>().AnyAsync())
            {
                //_context.Set<POSSetting>().Add(
                //    new POSSetting
                //    {
                //        Id = Guid.NewGuid(),
                //        DefaultVAT = 15m,
                //        //InvoiceHeader = "Admin",
                //        //IsActive = true
                //    }
                //);
                //await _context.SaveChangesAsync();
            }
        }
    }
}
