using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Shared.Wrapper;

using MediatR;
using OneOf;
using CleanArchTemplate.Domain.Customers;

namespace CleanArchTemplate.Application.Customers.Commands;
public record struct DeleteCustomerCommand(Guid Id):IRequest<OneOf<Result,Exception>>;
public sealed class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, OneOf<Result, Exception>>
{
    private readonly IApplicationDbContext _context;

    public DeleteCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result, Exception>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = new CustomerId(request.Id);
            var repo = _context.Set<Customer>();
            var found = await repo.FindAsync(id, cancellationToken);
            if (found is null) return new Exception($"Customer '({request.Id})' not found.");
            if (found.Deleted) return new Exception($"Customer ({request.Id}) is alrady deleted.");
            repo.Remove(found);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch(Exception ex)
        {
            
            return ex;
        }
    }
}
