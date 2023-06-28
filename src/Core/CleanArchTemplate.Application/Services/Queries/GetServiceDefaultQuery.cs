using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Domain.Services;
using CleanArchTemplate.Shared.Responses.Services;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using System.Linq;

namespace CleanArchTemplate.Application.Services.Queries;

public record struct GetServiceDefaultQuery()
    : IRequest<OneOf<Result<ServiceDefaultResponse>, Exception>>;

public sealed class GetServiceDefaultQueryHandler
    : IRequestHandler<GetServiceDefaultQuery, OneOf<Result<ServiceDefaultResponse>, Exception>>
{
    private readonly IApplicationDbContext _context;

    public GetServiceDefaultQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OneOf<Result<ServiceDefaultResponse>, Exception>> Handle(
        GetServiceDefaultQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var Services = await _context
                .Set<Service>()
                .Select(x => x.Map())
                .ToListAsync(cancellationToken);
            var routes = await _context
                .Set<Route>()
                .Select(x => x.Map())
                .ToListAsync(cancellationToken);
            var status = ServiceStatus.Supported.Select(x => x.Status);
            var response = new ServiceDefaultResponse(Services, routes,status);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
