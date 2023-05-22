namespace CleanArchTemplate.Domain.Invoices;

public abstract record VehicleType
{
    public static string Car = nameof(Car);
    public static string Truck = nameof(Truck);
    public static string Bus = nameof(Bus);

    public string Type { get; }

    public VehicleType(string type)
    {
        Type = type;
    }
    public static IEnumerable<VehicleType> Supported{
        get{
                yield return new BusType();
                yield return new CarType();
                yield return new TruckType();
            }
    }
    public static VehicleType Create(string type)
    {
        var found = Supported.SingleOrDefault(x => x.Type == type);
        if(found is null)   throw new ArgumentException($"Invalid vehicle type: {type}");
        return found;
    }
    public static implicit operator VehicleType(string type) => Create(type);
    public static implicit operator string(VehicleType type) => type.Type;
}

public record BusType() : VehicleType(VehicleType.Bus);
public record CarType() : VehicleType(VehicleType.Car);
public record TruckType() : VehicleType(VehicleType.Truck);