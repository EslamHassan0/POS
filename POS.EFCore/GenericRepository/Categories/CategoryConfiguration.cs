using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domain.Entity.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.EFCore.GenericRepository.ItemCategories;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(x => x.Id);

        // Properties

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.Description)
               .HasMaxLength(1000);

        builder.Property(x => x.IsActive)
               .HasDefaultValue(true);

        builder.Property(x => x.CreationTime)
               .IsRequired();

        //builder.HasMany(a => a.Items)
        //.WithOne(e => e.ItemCategory)
        //.HasForeignKey(e => e.ItemCategoryId)
        //.IsRequired();
    }
}
