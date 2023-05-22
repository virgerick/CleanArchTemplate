using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CleanArchTemplate.Domain.Invoices;

namespace CleanArchTemplate.Infrastructure.Persistences.Configurations.Vehicles;
public sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.Property(x => x.Id)
        .HasConversion(x => x.Value, value =>new VehicleId(value));
        builder.Property(x => x.Type)
        .HasConversion(x => x.Type, type => VehicleType.Create(type));
        builder.Property(x => x.Status)
        .HasConversion(x => x.Status, status => VehicleStatus.Create(status));

    }
}