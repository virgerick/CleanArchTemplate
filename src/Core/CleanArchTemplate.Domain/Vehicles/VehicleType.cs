using CleanArchTemplate.Domain.Common;

namespace CleanArchTemplate.Domain.Invoices;
public record struct VehicleTypeId(Guid Value);

public  class VehicleType :AuditableEntity<VehicleTypeId>
{
    private List<Model> _models=new();

    public string Name { get; set; }
    private VehicleType(){}
    private VehicleType(string name)
    {
        Name = name;
    }
    public IReadOnlyList<Model> Models { get => _models;  }


    public static VehicleType Create(string name)=>new VehicleType(name);
}

