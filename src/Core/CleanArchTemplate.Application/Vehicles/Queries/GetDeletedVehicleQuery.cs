using CleanArchTemplate.Application.Common.Extensions;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Queries;

public record struct GetDeletedVehicleQuery():IRequest<OneOf<ResultList<VehicleResponse>,Exception>>;

public sealed class GetDeletedVehicleQueryHandler : IRequestHandler<GetDeletedVehicleQuery, OneOf<ResultList<VehicleResponse>, Exception>>
{
    private readonly IApplicationDbContext _context;

    public GetDeletedVehicleQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<ResultList<VehicleResponse>, Exception>> Handle(GetDeletedVehicleQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _context.Set<Vehicle>()
                 .Where(x => x.Deleted)
                 .Select(x => x.Map())
                 .ToResultListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
