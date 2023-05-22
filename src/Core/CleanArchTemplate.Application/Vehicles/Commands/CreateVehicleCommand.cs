
using System.Collections.Generic;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Commands;

public record struct CreateVehicleCommand(string plateNumber, string brand, string model, string type):IRequest<OneOf<Guid,Exception>>;
public sealed class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;


    public CreateVehicleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Guid, Exception>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var repo =_context.Set<Vehicle>();
        var existed=await repo.AnyAsync(x => x.PlateNumber == request.plateNumber);
        if(existed) return new Exception($"There is an existing vehicle with the  plateNumber: '{request.plateNumber}' ");
        Vehicle create = null!;
        List<ValidationFailure> validationErrors=new();
        Vehicle.Create(request.plateNumber, request.brand, request.model, request.type)
        .Switch(
            vehicle => create = vehicle,
            errors => validationErrors.AddRange(errors)
        );
        if(validationErrors.Any()) 
            return new ValidationException(validationErrors);
        repo.Add(create);
        await _context.SaveChangesAsync(cancellationToken);
        return create.Id.Value;
    }
}
