using System.ComponentModel;
using System.Xml.Linq;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Drivers;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Drivers.Commands;

public record struct CreateDriverCommand(string Name, string PhoneNumber, string License, DateTime HireDate) :IRequest<OneOf<Guid,Exception>>;
public sealed class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public CreateDriverCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Guid, Exception>> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repo = _context.Set<Driver>();
            if (repo.Any(x => x.Name == request.Name)) return new Exception($"There is an existing driver named: '{request.Name}' ");
            Driver create = null!;
            List<ValidationFailure> validationErrors = new();
            Driver.Create(request.Name,request.PhoneNumber,request.License,request.HireDate)
            .Switch(
                Driver => create = Driver,
                validationErrors.AddRange
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
