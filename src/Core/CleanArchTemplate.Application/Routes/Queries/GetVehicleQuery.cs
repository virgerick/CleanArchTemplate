using CleanArchTemplate.Application.Common.Extensions;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Queries;
public record struct GetVehicleQuery():IRequest<OneOf<ResultList<VehicleResponse>,Exception>>;
public sealed class GetVehicleQueryHandler : IRequestHandler<GetVehicleQuery, OneOf<ResultList<VehicleResponse>, Exception>>
{
    private readonly IApplicationDbContext _context;

    public GetVehicleQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<ResultList<VehicleResponse>, Exception>> Handle(GetVehicleQuery request, CancellationToken cancellationToken)
    {
        try
        {
           return await _context.Set<Vehicle>()
                //.Select(x => new VehicleResponse(x.Id.Value, x.PlateNumber, x.Brand, x.Model, x.Type, x.Status,x.Deleted))
                .Select(x => x.Map())
                .ToResultListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
