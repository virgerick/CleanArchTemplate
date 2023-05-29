using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Commands;

public record struct MaintenanceVehicleCommand(Guid Id) : IRequest<OneOf<Result, Exception>>;
public sealed class MaintenanceVehicleCommandHandler : IRequestHandler<MaintenanceVehicleCommand, OneOf<Result, Exception>>
{
    private readonly IApplicationDbContext _context;

    public MaintenanceVehicleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OneOf<Result, Exception>> Handle(MaintenanceVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await _context.Set<Vehicle>()
            .SingleOrDefaultAsync(x => x.Id.Value == request.Id,cancellationToken);
            if(vehicle is null) return new Exception($"Vehicle '({request.Id})' not found.");
            vehicle.Maintenance();
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }
        catch (System.Exception ex)
        {
            return ex;
        }
    }
}
