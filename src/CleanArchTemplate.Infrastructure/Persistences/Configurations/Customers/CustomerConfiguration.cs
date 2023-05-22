using CleanArchTemplate.Domain.Customers;
using CleanArchTemplate.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchTemplate.Infrastructure.Persistences.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(x => x.Id)
             .HasConversion(x => x.Value, value => new CustomerId(value));
        builder.Property(x => x.Address)
            .HasConversion(x => x.ToJsonSerialize(), json => json.ToDeserialize<Address>());

          builder.Metadata.FindNavigation(nameof(Customer.Contracts))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}

