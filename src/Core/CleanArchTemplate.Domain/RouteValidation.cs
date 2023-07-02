using FluentValidation;

namespace CleanArchTemplate.Domain.Routes;

public sealed class RouteValidation : AbstractValidator<Route>
{
    public RouteValidation()
    {
        RuleFor(x => x.Origin).NotEmpty().WithMessage("Origin is required");
        RuleFor(x => x.Destination).NotEmpty().WithMessage("Destination is required");
        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("Amount is required");
    }
}
