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
    public ModelId ModelId { get; set; }
    public string Color { get; private set; }
    public VehicleStatus Status { get; private set; }
    public Model Model { get; private set; }
    public ICollection<Service> Services { get; private set; }

    protected Vehicle(){} // Constructor protegido para EF Core

    public static OneOf<Vehicle,List<ValidationFailure>> Create(string plateNumber,  ModelId modelId, string color)
    {
        var validator = new VehicleValidation();
        var vehicle= new Vehicle { Id = VehicleId.NewId(), PlateNumber = plateNumber, ModelId = modelId, Color=color,Status = new AvailableStatus() };
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
        Status = VehicleStatus.OutOfService;
    }
    public void Activate()
    {
        Status = VehicleStatus.Available;
    }
    public void Maintenance(){
        Status= VehicleStatus.Maintenance;;
    }
    public OneOf<bool,List<ValidationFailure>> Update(string plateNumber,ModelId modelId, string color)
    {
        var changed = false;
        if (PlateNumber != plateNumber)
        {
            PlateNumber = plateNumber;
            changed = true;
        }
        if(!modelId.Equals(ModelId)){
            ModelId=modelId;
            Model = null!;
            changed = true;
        }
        if (color != Color)
        {
            Color = color;
            changed = true;
        }
        var validationResult=new VehicleValidation().Validate(this);
        if(!validationResult.IsValid) return validationResult.Errors;
        return changed;
    }
}
public class VehicleValidation:AbstractValidator<Vehicle>{
    public VehicleValidation()
    {
        RuleFor(x => x.PlateNumber)
        .NotNull()
        .NotEmpty();
    
        RuleFor(x => x.ModelId.Value)
        .NotNull()
        .NotEmpty();
        
    }
}