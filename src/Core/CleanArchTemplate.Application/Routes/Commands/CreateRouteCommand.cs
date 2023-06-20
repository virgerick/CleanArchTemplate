using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Shared.Wrapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Routes.Commands;


public record struct CreateRouteCommand(string Origin, string Destination, float Distance, float EstimatedTime, decimal Amount, Guid VehicleId):IRequest<OneOf<Guid,Exception>>;
public sealed class CreateRouteCommandHandler : IRequestHandler<CreateRouteCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public CreateRouteCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Guid, Exception>> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repo = _context.Set<Route>();
            //valid if and route with same origin and destination is not created before and amount.
            var isExist = await repo.AnyAsync(x => x.Origin == request.Origin && x.Destination == request.Destination && x.Amount == request.Amount, cancellationToken);
            if (isExist)
                return new ValidationException(new List<ValidationFailure> { 
                    new ValidationFailure("Origin", "Origin must be unique"),
                    new ValidationFailure("Destination", "Destination must be unique"),
                    new ValidationFailure("Amount", "Amount must be unique")
                }); 
            Route create = null!;
            List<ValidationFailure> validationErrors = new();
            var vehicleId =new VehicleId(request.VehicleId);
            Route.Create(request.Origin, request.Destination, request.Distance, request.EstimatedTime, request.Amount,vehicleId)
            .Switch(
                Route => create = Route,
                validationErrors.AddRange
            );
            if (validationErrors.Any())
                return new ValidationException(validationErrors);
            repo.Add(create);
            await _context.SaveChangesAsync(cancellationToken);
            return create.Id.Value;
        }
        catch (Exception ex)
        {
            return ex;
        }

    }
}
