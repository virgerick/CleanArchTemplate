using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Shared.Responses.Routes;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Routes.Queries;
public record struct GetRouteByIdQuery(Guid Id):IRequest<OneOf<Result<RouteResponse>,Exception>>;
public sealed class GetRouteByIdQueryHandler : IRequestHandler<GetRouteByIdQuery, OneOf<Result<RouteResponse>, Exception>>
{
    private readonly IApplicationDbContext _context;

    public GetRouteByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result<RouteResponse>, Exception>> Handle(GetRouteByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var RouteId = new RouteId(request.Id);
            var found = await _context.Set<Route>()
                .Where(x =>  x.Id == RouteId)
            .Select(x => x.Map())
            .FirstOrDefaultAsync(cancellationToken);
            if (found == default) return new Exception($"Route ({request.Id}) was not found.");
            return Result<RouteResponse>.Success(found);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}