using CleanArchTemplate.Domain.Contracts;
using CleanArchTemplate.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchTemplate.Infrastructure.Persistences.Configurations;

public sealed class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.Property(x => x.Id)
             .HasConversion(x => x.Value, value => new ContractId(value));
        builder.Property(x => x.Type)
             .HasConversion(x => x.Type, value => ContractType.Create(value));
        builder.Property(x => x.CustomerId)
        .HasConversion(x => x.Value, value => new CustomerId(value));
        builder.Metadata.FindNavigation(nameof(Contract.Services))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
