using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Commands;

public record struct DeactivateVehicleCommand(Guid Id) : IRequest<OneOf<Result, Exception>>;
public sealed class DeactivateVehicleCommandHandler : IRequestHandler<DeactivateVehicleCommand, OneOf<Result, Exception>>
{
    private readonly IApplicationDbContext _context;

    public DeactivateVehicleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OneOf<Result, Exception>> Handle(DeactivateVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = new VehicleId(request.Id);
            var vehicle = await _context.Set<Vehicle>()
            .SingleOrDefaultAsync(x => x.Id == id,cancellationToken);
            if(vehicle is null) return new Exception($"Vehicle '({request.Id})' not found.");
            vehicle.Deactivate();
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }
        catch (System.Exception ex)
        {
            return ex;
        }
    }
}
