using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Invoices;
using OneOf;
using FluentValidation;
using FluentValidation.Results;

namespace CleanArchTemplate.Domain.Routes;
public record struct RouteId(Guid Value) {
    public static RouteId NewId() => new RouteId(Guid.NewGuid());
};
public class Route : AuditableEntity<RouteId>
{
    private Route() { }
    public string Origin { get; private set; }
    public string Destination { get; private set; }
    public float Distance { get; private set; }
    public float EstimatedTime { get; private set; }
    public decimal Amount { get; private set; }
    public VehicleId VehicleId { get; private set; }
    public Vehicle? Vehicle { get; private set; }

    public static OneOf<Route, IEnumerable<ValidationFailure>> Create(string origin, string destination, float distance, float estimatedTime, decimal amount, VehicleId vehicleId)
    {
        var route = new Route()
        {
            Id = RouteId.NewId(),
            Origin = origin,
            Destination = destination,
            Distance = distance,
            EstimatedTime = estimatedTime,
            Amount = amount,
            VehicleId = vehicleId
        };
        var validationResult = new RouteValidation().Validate(route);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors;
        }
        return route;
    }
    //create a update method that return true if any field was changed
    public OneOf<bool,IEnumerable<ValidationFailure>> Update(string origin, string destination, float distance, float estimatedTime, decimal amount, VehicleId vehicleId)
    {
        var changed = false;
        if (origin != Origin)
        {
            Origin = origin;
            changed = true;
        }
        if (destination != Destination)
        {
            Destination = destination;
            changed = true;
        }
        if (distance != Distance)
        {
            Distance = distance;
            changed = true;
        }
        if (estimatedTime != EstimatedTime)
        {
            EstimatedTime = estimatedTime;
            changed = true;
        }
        if (amount != Amount)
        {
            Amount = amount;
            changed = true;
        }
        if (vehicleId != VehicleId)
        {
            VehicleId = vehicleId;
            changed = true;
        }
        var validationResult = new RouteValidation().Validate(this);
        if (!validationResult.IsValid)
            return validationResult.Errors;
        return changed;

    }
}



