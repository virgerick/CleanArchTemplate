using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain.Customers;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses.Customers;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Customers.Queries;

public record struct GetCustomerByIdQuery(Guid Id):IRequest<OneOf<Result<CustomerResponse>,Exception>>;
public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, OneOf<Result<CustomerResponse>, Exception>>
{
    private readonly IApplicationDbContext _context;

    public GetCustomerByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result<CustomerResponse>, Exception>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var CustomerId = new CustomerId(request.Id);
            var found = await _context.Set<Customer>()
                .Where(x => !x.Deleted && x.Id == CustomerId)
            .Select(x => x.Map())
            .FirstOrDefaultAsync(cancellationToken);
            if (found is null) return new Exception($"Customer ({request.Id}) was not found.");
            return Result<CustomerResponse>.Success(found);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}