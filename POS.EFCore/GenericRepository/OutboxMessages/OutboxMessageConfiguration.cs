using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domain.Entity.OutboxMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.EFCore.GenericRepository.OutboxMessages;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
               .IsRequired();

        builder.Property(x => x.Payload)
               .HasColumnType("nvarchar(max)")
               .IsRequired();

        builder.Property(x => x.Status)
               .HasConversion<int>();

        builder.Property(x => x.LastError)
               .HasMaxLength(4000);

        builder.HasIndex(x => new
        {
            x.Status,
            x.AvailableAt
        });

        builder.HasIndex(x => x.AggregateId);
    }
}
