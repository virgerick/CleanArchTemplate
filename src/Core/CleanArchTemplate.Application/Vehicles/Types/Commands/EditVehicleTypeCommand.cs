using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using OneOf;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;

namespace CleanArchTemplate.Application.Vehicles.Types.Commands;
public record struct EditVehicleTypeCommand(Guid Id, string Name) : IRequest<OneOf<Guid, Exception>>;
public sealed class EditVehicleTypeCommandHandler : IRequestHandler<EditVehicleTypeCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public EditVehicleTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;

    }
    public async Task<OneOf<Guid, Exception>> Handle(EditVehicleTypeCommand request, CancellationToken cancellationToken)
    {
        var repo = _context.Set<VehicleType>();
        var VehicleTypeId = new VehicleTypeId(request.Id);
        var found = await repo
        .SingleOrDefaultAsync(x=>x.Id == VehicleTypeId, cancellationToken);
        if(found is null) return new Exception($"The VehicleType ('{request.Id}') was not found.");
        var existingPlate =await repo.AnyAsync(x => x.Name == request.Name && x.Id != VehicleTypeId);
        if(existingPlate) return new Exception($"There is an existing VehicleType with the  plateNumber: '{request.Name}'.");
        List<ValidationFailure> validationErrors = new();
        bool hasChange = found.Update(request.Name);
        
        if(hasChange){
            await _context.SaveChangesAsync(cancellationToken);
        }

        return found.Id.Value;
    }
}
