using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Types.Commands;

public record struct CreateVehicleTypeCommand(string Name):IRequest<OneOf<Guid,Exception>>;
public sealed class CreateVehicleTypeCommandHandler : IRequestHandler<CreateVehicleTypeCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public CreateVehicleTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Guid, Exception>> Handle(CreateVehicleTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repo = _context.Set<VehicleType>();
            var existed = await repo.AnyAsync(x => x.Name == request.Name);
            if (existed) return new Exception($"There is an existing VehicleType with the  name: '{request.Name}' ");
            VehicleType create = null!;
            List<ValidationFailure> validationErrors = new();
            create=VehicleType.Create(request.Name);
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
