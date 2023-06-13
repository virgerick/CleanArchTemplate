using System.Reflection.Metadata.Ecma335;
using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Invoices;

namespace CleanArchTemplate.Domain;
public record struct BrandId(Guid Value)
{
    public static BrandId NewId() => new BrandId(Guid.NewGuid());
}

public class Brand:AuditableEntity<BrandId>
{
    private List<Model> _models = new();
    public string Name { get; private set; }
    public string? Logo { get; private set; }
    public IReadOnlyList<Model> Models { get => _models; }

    public static Brand Create(string name, string? logo)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        return new() {Id=BrandId.NewId(), Name = name, Logo = logo };
    }
    public bool Update(string name, string? logo)
    {
        var change = false;
        ArgumentException.ThrowIfNullOrEmpty(name);

        if (Name != name) {
            Name = name;
            change = true;
        }
        if(Logo!= logo) {
            Logo = logo;
            change = true;
        }
        return change;
    }
}

public record struct ModelId(Guid Value);
public class Model:AuditableEntity<ModelId> 
{
    private List<Vehicle> _vehicles =new();
    public BrandId BrandId { get; private set; }
    public Brand Brand { get;private set; }
    public string Name { get; private set; }
    public int Year { get; private set; }
    public VehicleTypeId TypeId { get;private set;}
    public VehicleType Type { get;private set;}
    public IReadOnlyList<Vehicle> Vehicles => _vehicles;  
}