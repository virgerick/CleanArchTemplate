
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
            .HasConstraintName("FK_Transactions_Accounts_OriginAccountId")
            .HasForeignKey(x => x.OriginAccountId)
            .OnDelete(DeleteBehavior.NoAction);
            
        builder.HasOne(x => x.DestinationAccount)
            .WithMany()
             .HasConstraintName("FK_Transactions_Accounts_DestinationAccountId")
            .HasForeignKey(x => x.DestinationAccountId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
