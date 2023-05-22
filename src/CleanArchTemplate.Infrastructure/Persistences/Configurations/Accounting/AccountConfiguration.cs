
using CleanArchTemplate.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchTemplate.Infrastructure.Persistences.Configurations;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, value => new AccountId(value));
        builder.Property(x => x.Type)
            .HasConversion(x => x.Type, value => AccountType.Create(value));
            
    }
}


