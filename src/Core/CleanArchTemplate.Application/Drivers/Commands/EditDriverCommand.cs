using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Drivers;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Drivers.Commands;

public record struct EditDriverCommand(Guid Id, string Name, string PhoneNumber, string License, DateTime HireDate) : IRequest<OneOf<Guid, Exception>>;
public sealed class EditDriverCommandHandler : IRequestHandler<EditDriverCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public EditDriverCommandHandler(IApplicationDbContext context)
    {
        _context = context;

    }
    public async Task<OneOf<Guid, Exception>> Handle(EditDriverCommand request, CancellationToken cancellationToken)
    {
        var repo = _context.Set<Driver>();
        var DriverId = new DriverId(request.Id);
        var found = await repo
        .SingleOrDefaultAsync(x => x.Id == DriverId, cancellationToken);
        if (found is null) return new Exception($"The Driver ('{request.Id}') was not found.");
        List<ValidationFailure> validationErrors = new();
        bool hasChange = false;
        found.Update(request.Name, request.PhoneNumber, request.License, request.HireDate)
        .Switch(
            value => hasChange = value,
            validationErrors.AddRange
        );
        if (validationErrors.Any())
            return new ValidationException(validationErrors);

        if (hasChange)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        return found.Id.Value;
    }
}
