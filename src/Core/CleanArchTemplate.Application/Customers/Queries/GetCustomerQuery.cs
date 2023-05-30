using CleanArchTemplate.Application.Common.Extensions;
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
public record struct GetCustomerQuery():IRequest<OneOf<ResultList<CustomerResponse>,Exception>>;
public sealed class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, OneOf<ResultList<CustomerResponse>, Exception>>
{
    private readonly IApplicationDbContext _context;

    public GetCustomerQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<ResultList<CustomerResponse>, Exception>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        try
        {
           return await _context.Set<Customer>()
                .Select(x => x.Map())
                .ToResultListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}