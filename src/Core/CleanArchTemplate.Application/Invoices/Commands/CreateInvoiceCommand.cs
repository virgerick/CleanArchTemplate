
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain.Customers;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Domain.Services;
using CleanArchTemplate.Shared.Requests.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchTemplate.Application.Invoices.Commands;

public sealed record  CreateInvoiceCommand(Guid CustomerId, DateTime IssueDate,bool SaveAsDraft, InvoiceLineRequest[] Lines) : CreateInvoiceRequest(CustomerId,IssueDate,SaveAsDraft,Lines), IRequest<Result<Guid>>;

public sealed class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateInvoiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public  Task<Result<Guid>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        => Result<Guid>.TryCatch(async () =>{
            var( invoice, errors) = await CreateInvoiceAsync(request, cancellationToken);
            if(errors.Any())
                return await Result<Guid>.FailureAsync(errors.Select(x => x.ErrorMessage));

            await AddServiceLinesAsync(invoice, request,  errors, cancellationToken);
            await AddRouteLinesAsync(invoice, request,  errors, cancellationToken);
            if (errors.Any())
            {
                return await Result<Guid>.FailureAsync(errors.Select(x=>x.ErrorMessage));
            }
            await SaveInvoice(invoice, cancellationToken);
            return invoice.Id.Value;
        });
    

    private async Task SaveInvoice(Invoice invoice, CancellationToken cancellationToken)
    {
        var invoiceRepo = _context.Set<Invoice>();
        await invoiceRepo.AddAsync(invoice, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<(Invoice, List<ValidationFailure>)> CreateInvoiceAsync(CreateInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var errors = new List<ValidationFailure>();
        Invoice invoice = null!;
        var (customer, issueDate, saveAsDraft, invoiceLinea) = request;
        if (!invoiceLinea.Any())
            errors.Add(new ValidationFailure("Lines", "Invoice must have at least one line."));

        var customerId = new CustomerId(customer);
        if (!await _context.Set<Customer>().AnyAsync(x => x.Id == customerId, cancellationToken))
        {
            errors.Add(new ValidationFailure("CustomerId", $"Customer '{customerId.Value}' not found"));
        }
        Invoice.Create(issueDate, customerId, saveAsDraft ? new DraftStatus() : null)
            .Switch(create => invoice = create, errors.AddRange);
        return (invoice, errors);
    }

    private async Task AddServiceLinesAsync(Invoice invoice,
        CreateInvoiceCommand request,
        List<ValidationFailure> errors,
        CancellationToken cancellationToken)
    {
        var (_, _, _, invoiceLines) = request;
        var servicesIds = invoiceLines.Where(x => x.ServiceId != Guid.Empty).Select(x => new ServiceId(x.ServiceId));
        var services = await _context.Set<Service>()
            .Where(x => servicesIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
        if (!services.Any()) return;
        foreach (var service in services)
        {
            var requestLine = invoiceLines.FirstOrDefault(r => r.ServiceId == service.Id.Value);
            if (requestLine == null) continue;
            
            InvoiceLine.Create(invoice.Id, service.Id, service.Name, service.Amount, requestLine.Quantity)
                .Switch(invoice.AddInvoiceLine, errors.AddRange);
        }
    }

    private async Task AddRouteLinesAsync(Invoice invoice, CreateInvoiceCommand request,  List<ValidationFailure> errors,
        CancellationToken cancellationToken)
    {
        var (_, _, _, invoiceLines) = request;
        var routesIds = invoiceLines.Where(x => x.RouteId != Guid.Empty)
            .Select(x => new RouteId(x.RouteId));
        var routes = await _context.Set<Route>()
            .Where(x => routesIds.Contains(x.Id)).ToListAsync(cancellationToken);
        if (!routes.Any()) return;
        foreach (var route in routes)
        {
            var description = $"{route.Origin}/{route.Destination}";
            var requestLine = invoiceLines.FirstOrDefault(r => r.RouteId == route.Id.Value);
            if (requestLine == null) continue;
            InvoiceLine.Create(invoice.Id, route.Id, description, route.Amount, requestLine.Quantity)
                .Switch(invoice.AddInvoiceLine, errors.AddRange);
        }
    }

}