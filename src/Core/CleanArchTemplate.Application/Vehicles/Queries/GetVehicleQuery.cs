using CleanArchTemplate.Application.Common.Extensions;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            .Select(x => new VehicleResponse(x.Id.Value, x.PlateNumber, x.Brand, x.Model, x.Type, x.Status))
            .ToResultListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
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
           var found = await _context.Set<Vehicle>()
                .Where(x=>x.Id.Value==request.Id)
            .Select(x => new VehicleResponse(x.Id.Value, x.PlateNumber, x.Brand, x.Model, x.Type, x.Status))
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