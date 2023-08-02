using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Invoices;
using OneOf;
using FluentValidation;
using FluentValidation.Results;

namespace CleanArchTemplate.Domain.Routes;
public record  struct RouteId(Guid Value) {
    public static readonly RouteId Empty = new RouteId(Guid.Empty);
    public static RouteId NewId() => new RouteId(Guid.NewGuid());
};
public class Route : AuditableEntity<RouteId>
{
    public static readonly Route Empty = new Route
    {
        Id = RouteId.Empty,
        Origin = "Nowhere",
        Destination = "Nowhere",
        Amount = 0
    };
    private Route() { }
    public string Origin { get; private set; }
    public string Destination { get; private set; }
    public decimal Amount { get; private set; }
    public List<Vehicle> Vehicles { get; private set; } = new();
    public List<InvoiceLine> InvoiceLines { get; private set; } = new();
    public static OneOf<Route, IEnumerable<ValidationFailure>> Create(string origin, string destination, decimal amount)
    {
        var route = new Route()
        {
            Id = RouteId.NewId(),
            Origin = origin,
            Destination = destination,
            Amount = amount,
          
        };
        var validationResult = new RouteValidation().Validate(route);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors;
        }
        return route;
    }
   
    public OneOf<bool,IEnumerable<ValidationFailure>> Update(string origin, string destination,  decimal amount)
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
        if (amount != Amount)
        {
            Amount = amount;
            changed = true;
        }
        var validationResult = new RouteValidation().Validate(this);
        if (!validationResult.IsValid)
            return validationResult.Errors;
        return changed;

    }
}



