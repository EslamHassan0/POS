using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domain.Entity.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.EFCore.GenericRepository.Items;

internal class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");

        // Primary Key
        builder.HasKey(x => x.Id);

        builder.HasIndex(a => a.Code)
            .IsUnique();
        builder.HasIndex(a => a.Barcode)
        .IsUnique();


        builder.Property(x => x.Name)
                 .IsRequired()
                 .HasMaxLength(50);

        builder.Property(x => x.Code)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(x => x.Barcode)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(x => x.Price)
               .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Cost)
               .HasColumnType("decimal(18,2)");

        builder.Property(x => x.VatPercent)
                 .HasColumnType("decimal(5,2)");

        builder.Property(x => x.IsVATEligible)
               .IsRequired();

        builder.Property(x => x.IsActive)
               .HasDefaultValue(true);

        builder.Property(x => x.Image)
                   .HasMaxLength(50);

        // Enum
        builder.Property(x => x.Unit)
               .IsRequired();

        builder.HasOne(a => a.Category)
            .WithMany()
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a=>a.InvoiceItems)
            .WithOne(a=>a.Item)
            .HasForeignKey(a=>a.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // --------------------
        // Concurrency
        // --------------------

        builder.Property(x => x.RowVersion)
               .IsRowVersion()
               .IsConcurrencyToken();


    }

}
