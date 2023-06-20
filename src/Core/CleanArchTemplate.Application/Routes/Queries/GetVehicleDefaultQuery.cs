using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Shared.Responses.Routes;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using System.Linq;
namespace CleanArchTemplate.Application.Routes.Queries;

public record struct GetRouteDefaultQuery():IRequest<OneOf<Result<RouteDefaultResponse>,Exception>>;
public sealed class GetRouteDefaultQueryHandler : IRequestHandler<GetRouteDefaultQuery, OneOf<Result<RouteDefaultResponse>, Exception>>
{
    private readonly IApplicationDbContext _context;

    public GetRouteDefaultQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OneOf<Result<RouteDefaultResponse>, Exception>> Handle(GetRouteDefaultQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var Routes= await _context.Set<Route>()
                 .Select(x => x.Map())
                 .ToListAsync(cancellationToken);
            var vehicles= await _context.Set<Vehicle>()
                 .Select(x => x.Map())
                 .ToListAsync(cancellationToken);
                 var places = Routes.Select(x => x.Origin).Union(Routes.Select(x => x.Destination)).Distinct();
            var response = new RouteDefaultResponse(Routes, vehicles,places);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}