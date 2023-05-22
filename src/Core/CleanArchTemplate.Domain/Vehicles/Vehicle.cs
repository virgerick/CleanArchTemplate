using System.Diagnostics.Contracts;
using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Services;
using FluentValidation;
using FluentValidation.Results;
using OneOf;

namespace CleanArchTemplate.Domain.Invoices;
public record struct VehicleId(Guid Value){
    public static VehicleId NewId() => new VehicleId(Guid.NewGuid());
};
public class Vehicle:AuditableRootEntity<VehicleId>
{
    public string PlateNumber { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public VehicleType Type { get; private set; }
    public VehicleStatus Status { get; private set; }
    public ICollection<Service> Services { get; private set; }

    protected Vehicle() {
        Id = VehicleId.NewId();
    } // Constructor protegido para EF Core

    public static OneOf<Vehicle,List<ValidationFailure>> Create(string plateNumber, string brand, string model, string type)
    {
        var validator = new VehicleValidation();
        var vehicle= new Vehicle { PlateNumber = plateNumber, Brand = brand, Model = model, Type = type, Status = new AvailableStatus() };
        var validationResult= validator.Validate(vehicle);
        if(!validationResult.IsValid) return validationResult.Errors;
        return vehicle;
    }

    public void UpdatePlateNumber(string newPlateNumber)
    {
        if (string.IsNullOrWhiteSpace(newPlateNumber))
        {
            throw new ArgumentException("Plate number must not be empty.", nameof(newPlateNumber));
        }

        PlateNumber = newPlateNumber;
    }

    public void Deactivate()
    {
        Status = new  OutOfServiceStatus();
    }

    public OneOf<bool,List<ValidationFailure>> Update(string plateNumber, string brand, string model, string type)
    {
        var changed = false;
        if (PlateNumber != plateNumber)
        {
            PlateNumber = plateNumber;
            changed = true;
        }
        if(Brand != brand){
            Brand = brand;
            changed = true;
        }
        if(Model != model){
            changed = true;
            Model = model;
        }
        if(Type != type){
            Type = type;
            changed = true;
        }

        var validationResult=new VehicleValidation().Validate(this);
        if(!validationResult.IsValid) return validationResult.Errors;
        return changed;
    }
}
file class VehicleValidation:AbstractValidator<Vehicle>{
    public VehicleValidation()
    {
        RuleFor(x => x.PlateNumber)
        .NotNull()
        .NotEmpty();
    
        RuleFor(x => x.Brand)
        .NotNull()
        .NotEmpty();
    
        RuleFor(x => x.Model)
        .NotNull()
        .NotEmpty();
        
    }
}