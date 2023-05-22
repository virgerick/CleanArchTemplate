
using CleanArchTemplate.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchTemplate.Infrastructure.Persistences.Configurations;

public sealed class TransationConfiguration : IEntityTypeConfiguration<Transaction>
{
 public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, value => new TransactionId(value));

        builder.HasOne(x => x.OriginAccount)
            .WithMany()
            .HasForeignKey(x => x.OriginAccountId);

        builder.HasOne(x => x.DestinationAccount)
            .WithMany()
            .HasForeignKey(x => x.DestinationAccountId);
    }
}
