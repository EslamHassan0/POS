using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domain.Entity.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.EFCore.GenericRepository.Invoices;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
    
            // =====================
            // TABLE NAME
            // =====================
            builder.ToTable("Invoices");

            // =====================
            // PRIMARY KEY
            // =====================
            builder.HasKey(x => x.Id);

            // =====================
            // PROPERTIES
            // =====================

            builder.Property(x => x.InvoiceNo)
                   .HasMaxLength(50);

            builder.Property(x => x.InvoiceDate)
                   .IsRequired();

            builder.Property(x => x.TotalBeforeTax)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.VatAmount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TotalAfterTax)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TotalAfterDiscount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Discount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.PaymentFee)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.GrandTotal)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.PaymentType)
                   .IsRequired();

            builder.Property(x => x.InvoiceStatus)
                   .IsRequired();

            // =====================
            // RELATIONSHIPS
            // =====================

            // Customer (Many Invoices → One Customer)
            builder.HasOne(x => x.Customer)
                   .WithMany(c => c.Invoices)
                   .HasForeignKey(x => x.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Payment (One Invoice → One Payment)
            builder.HasOne(x => x.Payment)
                   .WithOne()
                   .HasForeignKey<Invoice>(x => x.Id) // adjust if Payment has FK
                   .OnDelete(DeleteBehavior.Restrict);

            // InvoiceItems (One Invoice → Many Items)
            builder.HasMany(x => x.InvoiceItems)
                   .WithOne(i => i.Invoice)
                   .HasForeignKey(i => i.InvoiceId)
                   .OnDelete(DeleteBehavior.Cascade);

            //builder.HasIndex(x => x.InvoiceNo)
            //       .IsUnique();

            builder.HasIndex(x => x.CustomerId);
    
    }
}
