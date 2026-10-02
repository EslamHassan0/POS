using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domain.Entity.PurchaseOrders.PurchaseOrderItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.EFCore.GenericRepository.PurchaseOrders
{
    public class PurchaseOrderItemConfiguration : IEntityTypeConfiguration<PurchaseOrderItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
        {
            // =====================
            // TABLE NAME
            // =====================
            builder.ToTable("PurchaseOrderItems");

            // =====================
            // PRIMARY KEY
            // =====================
            builder.HasKey(x => x.Id);

            // =====================
            // PROPERTIES
            // =====================

            builder.Property(x => x.Qty)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(x => x.UnitPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(x => x.TotalLine)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            // =====================
            // RELATIONSHIPS
            // =====================

            // PurchaseOrderItem → Item (Many-to-One)
            builder.HasOne(x => x.Item)
                   .WithMany()
                   .HasForeignKey(x => x.ItemId)
                   .OnDelete(DeleteBehavior.Restrict);

            // =====================
            // INDEXES (important for POS)
            // =====================

            builder.HasIndex(x => x.PurchaseOrderId);

            builder.HasIndex(x => x.ItemId);
        }
    }
}
