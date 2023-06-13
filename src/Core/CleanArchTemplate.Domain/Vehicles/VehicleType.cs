using CleanArchTemplate.Domain.Common;

namespace CleanArchTemplate.Domain.Invoices;
public record struct VehicleTypeId(Guid Value) {
    public static VehicleTypeId NewId() => new(Guid.NewGuid());
};

public  class VehicleType :AuditableEntity<VehicleTypeId>
{
    private List<Model> _models=new();

    public string Name { get; private set; }
    private VehicleType(){}
    private VehicleType(string name)
    {
        Id = VehicleTypeId.NewId();
        Name = name;
    }
    public IReadOnlyList<Model> Models { get => _models;  }


    public static VehicleType Create(string name)=>new VehicleType(name);

    public bool Update(string name) {
        var change = false;
        if (Name != name) {
            Name = name;
            change = true;
        }
        return change;
    }
}

