using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Customers;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using OneOf;

namespace CleanArchTemplate.Application.Customers.Commands;

public record struct RestoreCustomerCommand(Guid Id):IRequest<OneOf<Result,Exception>>;
public sealed class RestoreCustomerCommandHandler : IRequestHandler<RestoreCustomerCommand, OneOf<Result, Exception>>
{
    private readonly IApplicationDbContext _context;

    public RestoreCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result, Exception>> Handle(RestoreCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = new CustomerId(request.Id);
            var repo = _context.Set<Customer>();
            var found = await repo.FindAsync(id, cancellationToken);
            if (found is null) return new Exception($"Customer '({request.Id})' not found.");
            found.ToRestore();
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch(Exception ex)
        {
            return ex;
        }
    }
}