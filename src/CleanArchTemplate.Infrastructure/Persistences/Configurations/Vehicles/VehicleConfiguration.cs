using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain;

namespace CleanArchTemplate.Infrastructure.Persistences.Configurations.Vehicles;
public sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.Property(x => x.Id)
        .HasConversion(x => x.Value, value =>new VehicleId(value));
        
        builder.Property(x => x.ModelId)
        .HasConversion(x => x.Value, value => new ModelId(value));
        
        builder.Property(x => x.Status)
        .HasConversion(x => x.Status, status => VehicleStatus.Create(status));

    }
}
public sealed class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.Property(x => x.Id)
        .HasConversion(x => x.Value, value =>new BrandId(value));
        
        builder.HasMany(x => x.Models)
        .WithOne(x=>x.Brand)
        .HasForeignKey(x => x.BrandId);
        

    }
}
public sealed class VehicleTypeConfiguration : IEntityTypeConfiguration<VehicleType>
{
    public void Configure(EntityTypeBuilder<VehicleType> builder)
    {
        builder.Property(x => x.Id)
        .HasConversion(x => x.Value, value =>new VehicleTypeId(value));

        builder.HasMany(x => x.Models)
               .WithOne(x => x.Type)
               .HasForeignKey(x => x.TypeId);        

    }
}
public sealed class ModelConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        builder.Property(x => x.Id)
        .HasConversion(x => x.Value, value =>new ModelId(value));

        builder.HasOne(x => x.Brand)
               .WithMany(x => x.Models)
               .HasForeignKey(x => x.BrandId);        

    }
}
