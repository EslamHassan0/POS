using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entity.Customers;
using POS.Domain.Entity.InvoiceItems;
using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.Categories;
using POS.Domain.Entity.Items;
using POS.Domain.Entity.Payments;
using POS.Domain.Entity.POSSettings;
using POS.Domain.Entity.PurchaseOrders;
using POS.Domain.Entity.PurchaseOrders.PurchaseOrderItems;
using POS.Domain.Entity.StockTransactions;
using POS.Domain.Entity.Suppliers;
using POS.Domain.Entity.Warehouses;
using POS.Domain.Identity;
using POS.Domain.Entity.OutboxMessages;

namespace POS.EFCore.POSContexts
{
    public class POSContext : IdentityDbContext<IdentityApplicationUser, IdentityRole<Guid>, Guid>
    {
        public POSContext(DbContextOptions<POSContext> options)
             : base(options)
        {
        }
        public DbSet<POSSetting> POSSettings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<StockTransaction>  StockTransactions { get; set; }
        public DbSet<Supplier>  Suppliers { get; set; }
        public DbSet<PurchaseOrder>  PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem>  PurchaseOrderItems { get; set; }
        public DbSet<Warehouse>  Warehouses { get; set; }
        public DbSet<OutboxMessage>  OutboxMessages { get; set; }
        
        //public DbSet<ApplicationUser>  ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(POSContext).Assembly);


            modelBuilder.HasSequence<int>("InvoiceNumberSequence")
                    .StartsAt(1)      
                    .IncrementsBy(1);  

            modelBuilder.Entity<IdentityApplicationUser>()
                .OwnsMany(u => u.RefreshTokens, rt =>
                {
                    rt.WithOwner();
                    rt.Property(r => r.Token);
                    rt.HasKey(r => r.Token); 
                });



            modelBuilder.Entity<Item>()
            .Property(p => p.Price)
            .IsConcurrencyToken();

            modelBuilder.Entity<Item>()
            .HasMany(a => a.InvoiceItems)
            .WithOne(e => e.Item)
            .HasForeignKey(e => e.ItemId)
            .IsRequired();

            modelBuilder.Entity<Invoice>()
            .HasMany(a => a.InvoiceItems)
            .WithOne(e => e.Invoice)
            .HasForeignKey(e => e.InvoiceId)
            .IsRequired();

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Payment)
                .WithOne(p => p.Invoice)
                .HasForeignKey<Payment>(p => p.InvoiceId);

            modelBuilder.Entity<PurchaseOrder>()
             .HasOne(p => p.Supplier)
             .WithMany()
             .HasForeignKey(p => p.SupplierId);

            modelBuilder.Entity<PurchaseOrder>()
             .HasOne<Warehouse>()
             .WithMany()
             .HasForeignKey(p => p.WarehouseId);

            //modelBuilder.Entity<PurchaseOrderItem>()
            //    .HasOne(pi => pi.PurchaseOrder)
            //    .WithMany()
            //    .HasForeignKey(pi => pi.PurchaseOrderId);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(pi => pi.Item)
                .WithMany()
                .HasForeignKey(pi => pi.ItemId);

           

        }
        

    }
}
