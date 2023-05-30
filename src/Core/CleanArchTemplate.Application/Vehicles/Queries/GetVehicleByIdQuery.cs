using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Queries;

public record struct GetVehicleByIdQuery(Guid Id):IRequest<OneOf<Result<VehicleResponse>,Exception>>;
public sealed class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, OneOf<Result<VehicleResponse>, Exception>>
{
    private readonly IApplicationDbContext _context;

    public GetVehicleByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result<VehicleResponse>, Exception>> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var vehicleId = new VehicleId(request.Id);
            var found = await _context.Set<Vehicle>()
                .Where(x => !x.Deleted && x.Id == vehicleId)
            .Select(x => new VehicleResponse(x.Id.Value, x.PlateNumber, x.Brand, x.Model, x.Type, x.Status,x.Deleted))
            .FirstOrDefaultAsync(cancellationToken);
            if (found is null) return new Exception($"Vehicle ({request.Id}) was not found.");
            return Result<VehicleResponse>.Success(found);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}