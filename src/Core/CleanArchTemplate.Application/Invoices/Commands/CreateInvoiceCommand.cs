using System.Net.Security;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain.Customers;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Domain.Services;
using CleanArchTemplate.Shared.Requests.Invoices;
using CleanArchTemplate.Shared.Results;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchTemplate.Application.Invoices.Commands;

public sealed record  CreateInvoiceCommand
    (Guid CustomerId, DateTime IssueDate, InvoiceLineRequest[] Lines) :
        CreateInvoiceRequest(CustomerId,IssueDate,Lines), IRequest<Result<Guid>>;

public sealed class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateInvoiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public  Task<Result<Guid>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        return Result.TryCatch<Guid>(async () =>
        {
            var invoiceRepo = _context.Set<Invoice>();
            var customerId = new CustomerId(request.CustomerId);
            if (!await _context.Set<Customer>().AnyAsync(x => x.Id == customerId, cancellationToken))
                return Error.Create("CUSTOMER.NOT_FOUND", $"Customer '{request.CustomerId}' not found");

            Invoice invoice = null!;
            var error = Error.None;
            Invoice.Create(request.IssueDate, customerId)
                .Switch(create => invoice = create, failures => error = failures.ToValidationError());
            if (error != Error.None) return error;
            var servicesIds = request.Lines.Where(x=>x.ServiceId!=Guid.Empty).Select(x => new ServiceId(x.ServiceId));
            var routesIds = request.Lines.Where(x=>x.RouteId!=Guid.Empty).Select(x => new RouteId(x.RouteId));
            var services = await _context.Set<Service>().Where(x => servicesIds.Contains(x.Id))
                .ToListAsync(cancellationToken);
            var errors = new List<ValidationFailure>();
            if (services.Any())
            {
                foreach (var service in services)
                {
                    var requestLine = request.Lines.FirstOrDefault(r => r.ServiceId == service.Id.Value);
                    if (requestLine != null)
                    {
                        InvoiceLine.Create(invoice.Id, service.Id, service.Name, service.Amount, requestLine.Quantity)
                            .Switch(line => invoice.AddInvoiceLine(line), errors.AddRange);
                    }
                }
            }

            var routes = await _context.Set<Route>().Where(x => routesIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (routes.Any())
            {

                foreach (var route in routes)
                {
                    var description = $"{route.Origin}-{route.Destination}";
                    var requestLine = request.Lines.FirstOrDefault(r => r.RouteId == route.Id.Value);
                    if (requestLine != null)
                    {
                        InvoiceLine.Create(invoice.Id, route.Id, description, route.Amount, requestLine.Quantity)
                            .Switch(line => invoice.AddInvoiceLine(line), errors.AddRange);

                    }
                }
            }

            if (errors.Any())
            {
                return errors.ToValidationError();
            }

            await invoiceRepo.AddAsync(invoice!, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return invoice.Id.Value;
        });
    }
}