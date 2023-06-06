using CleanArchTemplate.Application.Common.Extensions;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain.Services;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses.Services;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Services.Queries;
public record struct GetServiceQuery():IRequest<OneOf<ResultList<ServiceResponse>,Exception>>;
public sealed class GetServiceQueryHandler : IRequestHandler<GetServiceQuery, OneOf<ResultList<ServiceResponse>, Exception>>
{
    private readonly IApplicationDbContext _context;

    public GetServiceQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<ResultList<ServiceResponse>, Exception>> Handle(GetServiceQuery request, CancellationToken cancellationToken)
    {
        try
        {
           return await _context.Set<Service>()
                .Select(x => x.Map())
                .ToResultListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}