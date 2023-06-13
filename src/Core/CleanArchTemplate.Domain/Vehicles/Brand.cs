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

public record struct ModelId(Guid Value)
{
    public static ModelId NewId() => new ModelId(Guid.NewGuid());
}

public class Model:AuditableEntity<ModelId> 
{
    private List<Vehicle> _vehicles =new();
    private Model()
    {

    }
    private Model(string name, int year, BrandId brandId, VehicleTypeId typeId)
    {
        Id = ModelId.NewId();
        Name = name;
        Year = year;
        BrandId = brandId;
        TypeId = typeId;
    }

    public string Name { get; private set; }
    public int Year { get; private set; }
    public BrandId BrandId { get; private set; }
    public VehicleTypeId TypeId { get;private set;}
    public Brand Brand { get;private set; }
    public VehicleType Type { get;private set;}
    public IReadOnlyList<Vehicle> Vehicles => _vehicles;
    public static Model Create(string name, int year, BrandId brandId, VehicleTypeId typeId) => new(name, year,brandId,typeId);
    public bool Update(string name, int year, BrandId brandId, VehicleTypeId typeId) {
        var change = false;
        if (Name != name) {
            Name = name;
            change = true;
        }
        if (Year != year) {
            Year = year;
            change = true;
        }
        if (BrandId != brandId) {
            BrandId = brandId;
            change = true;
        }
        if (TypeId != typeId) {
            TypeId = typeId;
            change = true;
        }

        return change;
    }
}