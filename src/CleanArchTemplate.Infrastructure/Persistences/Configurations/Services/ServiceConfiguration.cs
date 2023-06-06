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

        builder.Property(x => x.RouteId)
       .HasConversion(x => x.Value, value => new RouteId(value));

        builder.HasOne(x => x.Route)
            .WithOne()
            .HasForeignKey<Service>(x => x.RouteId);

        builder.HasMany(x => x.Drivers)
            .WithMany(x => x.Services);
        builder.HasMany(x => x.Customers)
            .WithMany(x => x.Services);
            

    }
}