using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domain.Entity.Categories;
using POS.Domain.Entity.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.EFCore.GenericRepository.Customers;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{

    public void Configure(EntityTypeBuilder<Customer> b)
    {
        b.ToTable("Customers");

        b.HasKey(x => x.Id);

        // Properties
        b.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(200);

        b.Property(x => x.Phone)
               .IsRequired()
               .HasMaxLength(20);

        b.Property(x => x.Address)
               .HasMaxLength(500);

        b.HasMany(a=>a.Invoices)
            .WithOne(a=>a.Customer)
            .HasForeignKey(a=>a.CustomerId)
             .OnDelete(DeleteBehavior.Restrict);
    }
}
