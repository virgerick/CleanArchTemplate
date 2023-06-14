using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Queries;

public record struct GetVehicleDefaultQuery():IRequest<OneOf<Result<VehicleDefaultResponse>,Exception>>;
public sealed class GetVehicleDefaultQueryHandler : IRequestHandler<GetVehicleDefaultQuery, OneOf<Result<VehicleDefaultResponse>, Exception>>
{
    private readonly IApplicationDbContext _context;

    public GetVehicleDefaultQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OneOf<Result<VehicleDefaultResponse>, Exception>> Handle(GetVehicleDefaultQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var vehicles= await _context.Set<Vehicle>()
                 .Select(x => x.Map())
                 .ToListAsync(cancellationToken);
            var models= await _context.Set<Model>()
                 .Select(x => x.Map())
                 .ToListAsync(cancellationToken);
            var response = new VehicleDefaultResponse(vehicles.ToArray(), models.ToArray());
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}