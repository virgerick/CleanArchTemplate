using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Services;
using FluentValidation;
using FluentValidation.Results;
using OneOf;

namespace CleanArchTemplate.Domain.Drivers;

public class Driver : AuditableEntity<DriverId>
{
    private List<Service> _services=new();

    public string Name { get; private set; }
    public string PhoneNumber { get; private set; }
    public string License { get; private set; }
    public DateTime HireDate { get; private set; }
    public IReadOnlyList<Service> Services => _services;
    private Driver(){}
    public static OneOf<Driver, List<ValidationFailure>> Create(string name, string phoneNumber, string license, DateTime hireDate)
    {
        var driver = new Driver()
        {
            Id = DriverId.NewId(),
            Name = name,
            PhoneNumber = phoneNumber,
            License = license,
            HireDate = hireDate
        };
        var validationResult = new DriverValidator().Validate(driver);
        if (!validationResult.IsValid) return validationResult.Errors;

        return driver;
    }
    public OneOf<bool, List<ValidationFailure>> Update(string name, string phoneNumber, string license, DateTime hireDate)
    {
        var changed = false;
        if (Name != name)
        {
            Name = name;
            changed = true;
        }
        if (PhoneNumber != phoneNumber)
        {
            PhoneNumber = phoneNumber;
            changed = true;
        }
        if (License != license)
        {
            License = license;
            changed = true;
        }
        if (HireDate != hireDate)
        {
            HireDate = hireDate;
            changed = true;
        }
        var result = new DriverValidator().Validate(this);
        if (!result.IsValid) return result.Errors;

        return changed;

    }

}
public record struct DriverId(Guid Value) {
    public static DriverId NewId() => new DriverId(Guid.NewGuid());
};
file class DriverValidator : AbstractValidator<Driver>
{
    public DriverValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .MinimumLength(10);
        RuleFor(x => x.License)
            .NotNull()
            .NotEmpty();
    }
}
