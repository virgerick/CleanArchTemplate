using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Commands;

public record struct CreateVehicleCommand(string plateNumber,Guid modelId, string color):IRequest<OneOf<Guid,Exception>>;
public sealed class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public CreateVehicleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Guid, Exception>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repo = _context.Set<Vehicle>();
            var existed = await repo.AnyAsync(x => x.PlateNumber == request.plateNumber);
            if (existed) return new Exception($"There is an existing vehicle with the  plateNumber: '{request.plateNumber}' ");
            Vehicle create = null!;
            List<ValidationFailure> validationErrors = new();
            var modelId= new ModelId(request.modelId);
            Vehicle.Create(request.plateNumber,modelId,request.color)
            .Switch(
                vehicle => create = vehicle,
                errors => validationErrors.AddRange(errors)
            );
            if (validationErrors.Any())
                return new ValidationException(validationErrors);
            repo.Add(create);
            await _context.SaveChangesAsync(cancellationToken);
            return create.Id.Value;
        }
        catch (System.Exception ex)
        {
            return ex;
        }

    }
}
