using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Routes;
using Microsoft.EntityFrameworkCore;

namespace CleanArchTemplate.Infrastructure.Persistences.Configurations.Routes;
public sealed class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Route> builder)
    {
        builder.Property(x => x.Id)
        .HasConversion(x => x.Value, value => new RouteId(value));
         
        builder.HasMany(x => x.InvoiceLines)
            .WithOne(x => x.Route)
            .HasForeignKey(x=>x.RouteId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}