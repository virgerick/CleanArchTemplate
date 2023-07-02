using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchTemplate.Infrastructure.Persistences.Configurations.Services;
public sealed class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(x => x.Id)
        .HasConversion(x => x.Value, value => new ServiceId(value));
       
        builder.Property(x => x.Status)
        .HasConversion(x => x.Status, status => ServiceStatus.Create(status));

        builder.HasMany(x => x.Drivers)
            .WithMany(x => x.Services);
        builder.HasMany(x => x.Customers)
            .WithMany(x => x.Services);

        builder.HasMany(x => x.InvoiceLines)
            .WithOne(x => x.Service)
            .HasForeignKey(x=>x.ServiceId)
            .OnDelete(DeleteBehavior.NoAction);
            

    }
}