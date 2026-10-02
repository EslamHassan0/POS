using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domain.Entity.PurchaseOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.EFCore.GenericRepository.PurchaseOrders
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.ToTable("PurchaseOrders");

            builder.HasKey(x => x.Id);


            builder.Property(x => x.PurchaseOrderNo)
                   .HasMaxLength(50);

            builder.Property(x => x.Date)
                   .IsRequired();

            builder.Property(x => x.TotalAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(x => x.Status)
                   .IsRequired();

            builder.Property(x => x.SupplierId)
                   .IsRequired();

            builder.Property(x => x.WarehouseId)
                   .IsRequired();
 

            // Supplier (Many Orders → One Supplier)
            builder.HasOne(x => x.Supplier)
                   .WithMany()
                   .HasForeignKey(x => x.SupplierId)
                   .OnDelete(DeleteBehavior.Restrict);

            // PurchaseOrder → PurchaseOrderItems (1:N)
            builder.HasMany(x => x.PurchaseOrderItems)
                   .WithOne()
                   .HasForeignKey(x => x.PurchaseOrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.SupplierId);

            builder.HasIndex(x => x.WarehouseId);

            builder.HasIndex(x => x.Status);
        }
    }
}