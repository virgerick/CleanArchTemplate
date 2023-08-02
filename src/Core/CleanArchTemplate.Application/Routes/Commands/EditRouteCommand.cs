using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using OneOf;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Routes;

namespace CleanArchTemplate.Application.Routes.Commands;
public record struct EditRouteCommand(Guid Id, string Origin, string Destination, decimal Amount) : IRequest<OneOf<Guid, Exception>>;
public sealed class EditRouteCommandHandler : IRequestHandler<EditRouteCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public EditRouteCommandHandler(IApplicationDbContext context)
    {
        _context = context;

    }
    public async Task<OneOf<Guid, Exception>> Handle(EditRouteCommand request, CancellationToken cancellationToken)
    {
        var repo = _context.Set<Route>();
        var RouteId = new RouteId(request.Id);
        var found = await repo
        .SingleOrDefaultAsync(x=>x.Id == RouteId, cancellationToken);
        if(found is null) return new Exception($"The Route ('{request.Id}') was not found.");

         var isExist = await repo.AnyAsync(x =>x.Id!=found.Id && x.Origin == request.Origin && x.Destination == request.Destination && x.Amount == request.Amount, cancellationToken);
            if (isExist)
                return new ValidationException(new List<ValidationFailure> { 
                    new ValidationFailure("Origin", "Origin must be unique"),
                    new ValidationFailure("Destination", "Destination must be unique"),
                    new ValidationFailure("Amount", "Amount must be unique")
                }); 
        List<ValidationFailure> validationErrors = new();
        bool hasChange = false;
        found.Update(request.Origin, request.Destination, request.Amount)
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
