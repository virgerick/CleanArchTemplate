using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Customers;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Customers.Commands;

public record struct CreateCustomerCommand(string name, string email, string Street, string City, string State, string ZipCode) :IRequest<OneOf<Guid,Exception>>;
public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public CreateCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Guid, Exception>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repo = _context.Set<Customer>();
            Customer create = null!;
            Exception exception = null!;
            Customer.Create(request.name,request.email,new Address(request.Street,request.City,request.State,request.ZipCode))
            .Switch(
                Customer => create = Customer,
                error => exception=error
            );
            if (exception != null)
                return exception;
            repo.Add(create);
            await _context.SaveChangesAsync(cancellationToken);
            return create.Id.Value;
        }
        catch (System.Exception ex)
        {
            return ex;
        }

    }
}
