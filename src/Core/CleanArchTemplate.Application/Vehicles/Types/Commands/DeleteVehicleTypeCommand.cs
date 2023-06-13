using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Wrapper;

using MediatR;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Types.Commands;
public record struct DeleteVehicleTypeCommand(Guid Id):IRequest<OneOf<Result,Exception>>;
public sealed class DeleteVehicleTypeCommandHandler : IRequestHandler<DeleteVehicleTypeCommand, OneOf<Result, Exception>>
{
    private readonly IApplicationDbContext _context;

    public DeleteVehicleTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result, Exception>> Handle(DeleteVehicleTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = new VehicleTypeId(request.Id);
            var repo = _context.Set<VehicleType>();
            var found = await repo.FindAsync(id, cancellationToken);
            if (found is null) return new Exception($"VehicleType '({request.Id})' not found.");
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
