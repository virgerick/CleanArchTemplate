using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Shared.Wrapper;

using MediatR;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Commands;
public record struct DeleteVehicleCommand(Guid Id):IRequest<OneOf<Result,Exception>>;
public sealed class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, OneOf<Result, Exception>>
{
    private readonly IApplicationDbContext _context;

    public DeleteVehicleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result, Exception>> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = new VehicleId(request.Id);
            var repo = _context.Set<Vehicle>();
            var found = await repo.FindAsync(id, cancellationToken);
            if (found is null) return new Exception($"Vehicle '({request.Id})' not found.");
            if (found.Deleted) return new Exception($"Vehicle ({request.Id}) is alrady deleted.");
            repo.Remove(found);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch(Exception ex)
        {
            
            return ex;
        }
    }
}
