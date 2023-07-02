using CleanArchTemplate.Domain.Customers;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchTemplate.Infrastructure.Persistences.Configurations.Invoices;
public sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.Property(x => x.Id)
        .HasConversion(x => x.Value, value =>new InvoiceId(value));
         builder.Property(x => x.CustomerId)
             .HasConversion(x => x.Value, value => new CustomerId(value));
    }
}
public sealed class InvoiceLineConfiguration : IEntityTypeConfiguration<InvoiceLine>
{
    public void Configure(EntityTypeBuilder<InvoiceLine> builder)
    {
        builder.Property(x => x.Id)
        .HasConversion(x => x.Value, value =>new InvoiceLineId(value));
         builder.Property(x => x.InvoiceId)
        .HasConversion(x => x.Value, value =>new InvoiceId(value));
         builder.Property(x => x.ServiceId)
        .HasConversion(x => x!.Value, value =>new ServiceId(value));
         builder.Property(x => x.RouteId)
        .HasConversion(x => x!.Value, value =>new RouteId(value));
    }
}
