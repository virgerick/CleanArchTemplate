using CleanArchTemplate.Domain.Accounting;
using CleanArchTemplate.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchTemplate.Infrastructure.Persistences.Configurations
{
public sealed class AccountReceivableConfiguration : IEntityTypeConfiguration<AccountReceivable>
{
    public void Configure(EntityTypeBuilder<AccountReceivable> builder)
    {
        builder.Property(x => x.Id)
        .HasConversion(x => x.Value, value => new AccountReceivableId(value));
        builder.Property(x => x.CustomerId)
        .HasConversion(x => x.Value, value => new CustomerId(value));
    }
}
}