using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domain.Entity.Items;
using POS.Domain.Entity.StockTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.EFCore.GenericRepository.StockTransactions
{
    public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
    {
        public void Configure(EntityTypeBuilder<StockTransaction> builder)
        {
            // =====================
            // TABLE NAME
            // =====================
            builder.ToTable("StockTransactions");

            // =====================
            // PRIMARY KEY
            // =====================
            builder.HasKey(x => x.Id);

            // =====================
            // PROPERTIES
            // =====================

            builder.Property(x => x.QtyChange)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(x => x.TransactionType)
                   .IsRequired();

            builder.Property(x => x.ReferenceId)
                   .IsRequired();

            builder.Property(x => x.Date)
                   .IsRequired();

            // =====================
            // RELATIONSHIP
            // =====================

            builder.HasOne<Item>()
                   .WithMany()
                   .HasForeignKey(x => x.ItemId)
                   .OnDelete(DeleteBehavior.Restrict);

            // =====================
            // INDEXES (important for POS)
            // =====================

            builder.HasIndex(x => x.ItemId);

            builder.HasIndex(x => x.ReferenceId);

            builder.HasIndex(x => x.Date);
        }
    }
}
