using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Commands;

public record struct RestoreVehicleCommand(Guid Id):IRequest<OneOf<Result,Exception>>;
public sealed class RestoreVehicleCommandHandler : IRequestHandler<RestoreVehicleCommand, OneOf<Result, Exception>>
{
    private readonly IApplicationDbContext _context;

    public RestoreVehicleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result, Exception>> Handle(RestoreVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = new VehicleId(request.Id);
            var repo = _context.Set<Vehicle>();
            var found = await repo.FindAsync(id, cancellationToken);
            if (found is null) return new Exception($"Vehicle '({request.Id})' not found.");
            found.ToRestore();
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch(Exception ex)
        {
            return ex;
        }
    }
}