using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using OneOf;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Customers;

namespace CleanArchTemplate.Application.Customers.Commands;
public record struct EditCustomerCommand(Guid Id, string name, string email, string Street, string City, string State, string ZipCode) : IRequest<OneOf<Guid, Exception>>;
public sealed class EditCustomerCommandHandler : IRequestHandler<EditCustomerCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public EditCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;

    }
    public async Task<OneOf<Guid, Exception>> Handle(EditCustomerCommand request, CancellationToken cancellationToken)
    {
        var repo = _context.Set<Customer>();
        var CustomerId = new CustomerId(request.Id);
        var found = await repo
        .SingleOrDefaultAsync(x=>x.Id == CustomerId, cancellationToken);
        if(found is null) return new Exception($"The Customer ('{request.Id}') was not found.");
        Exception error= null!;
        bool hasChange = false;
        found.Update(request.name, request.email, new Address(request.Street, request.City, request.State, request.ZipCode))
        .Switch(
            value=>hasChange=value,
            err=>error=err
        );
        if(error !=null)
            return error;

        if(hasChange){
            await _context.SaveChangesAsync(cancellationToken);
        }

        return found.Id.Value;
    }
}
