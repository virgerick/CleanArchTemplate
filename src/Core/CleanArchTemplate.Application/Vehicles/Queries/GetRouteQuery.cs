using CleanArchTemplate.Application.Common.Extensions;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Shared.Responses.Routes;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using OneOf;

namespace CleanArchTemplate.Application.Routes.Queries;
public record struct GetRouteQuery():IRequest<OneOf<ResultList<RouteResponse>,Exception>>;
public sealed class GetRouteQueryHandler : IRequestHandler<GetRouteQuery, OneOf<ResultList<RouteResponse>, Exception>>
{
    private readonly IApplicationDbContext _context;

    public GetRouteQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<ResultList<RouteResponse>, Exception>> Handle(GetRouteQuery request, CancellationToken cancellationToken)
    {
        try
        {
           return await _context.Set<Route>()
                //.Select(x => new RouteResponse(x.Id.Value, x.PlateNumber, x.Brand, x.Model, x.Type, x.Status,x.Deleted))
                .Select(x => x.Map())
                .ToResultListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
