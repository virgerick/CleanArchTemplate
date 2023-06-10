using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using OneOf;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;

namespace CleanArchTemplate.Application.Vehicles.Commands;
public record struct EditVehicleCommand(Guid Id, string plateNumber, Guid modelId, string color) : IRequest<OneOf<Guid, Exception>>;
public sealed class EditVehicleCommandHandler : IRequestHandler<EditVehicleCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public EditVehicleCommandHandler(IApplicationDbContext context)
    {
        _context = context;

    }
    public async Task<OneOf<Guid, Exception>> Handle(EditVehicleCommand request, CancellationToken cancellationToken)
    {
        var repo = _context.Set<Vehicle>();
        var vehicleId = new VehicleId(request.Id);
        var found = await repo
        .SingleOrDefaultAsync(x=>x.Id == vehicleId, cancellationToken);
        if(found is null) return new Exception($"The Vehicle ('{request.Id}') was not found.");
        var existingPlate =await repo.AnyAsync(x => x.PlateNumber == request.plateNumber && x.Id != vehicleId);
        if(existingPlate) return new Exception($"There is an existing vehicle with the  plateNumber: '{request.plateNumber}'.");
        List<ValidationFailure> validationErrors = new();
        bool hasChange = false;
        var modelId = new Domain.ModelId(request.modelId);
        found.Update(request.plateNumber,modelId,request.color)
        .Switch(
            value=>hasChange=value,
            validationErrors.AddRange
        );
        if(validationErrors.Any())
            return new ValidationException(validationErrors);

        if(hasChange){
            await _context.SaveChangesAsync(cancellationToken);
        }

        return found.Id.Value;
    }
}
