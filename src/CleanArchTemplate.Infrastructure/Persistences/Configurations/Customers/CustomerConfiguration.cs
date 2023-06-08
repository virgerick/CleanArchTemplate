using CleanArchTemplate.Domain.Customers;
using CleanArchTemplate.Domain.Services;
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
        /*
        builder.Property(x => x.Services)
            .HasField("_services");
        builder.Property(x => x.Invoices)
            .HasField("_invoices");
        builder.Property(x => x.Contracts)
            .HasField("_contracts");*/

        builder.HasMany(c => c.Services)
            .WithMany(s => s.Customers)
            .UsingEntity<CustomerService>(customerBuilder =>{
            customerBuilder.Property(c => c.CustomerId).HasConversion(x => x.Value, value => new CustomerId(value));
            return customerBuilder.HasOne(c => c.Service).WithMany().HasForeignKey(c => c.ServiceId).OnDelete(DeleteBehavior.NoAction);
        },csbuilder => {
            csbuilder.HasKey(cs => new { cs.CustomerId, cs.ServiceId });
            csbuilder.Property(s => s.ServiceId).HasConversion(x => x.Value, value => new ServiceId(value));
            return csbuilder.HasOne(s => s.Customer).WithMany().HasForeignKey(s => s.CustomerId);
        });

    }
}

