namespace CleanArchTemplate.Domain.Invoices;

public abstract record VehicleType
{
    public static string Car = nameof(Car);
    public static string Truck = nameof(Truck);
    public static string Bus = nameof(Bus);

    public string Type { get; }

    public VehicleType(string type)
    {
        if (type != Car && type != Truck && type != Bus)
        {
            throw new ArgumentException($"Invalid vehicle type: {type}");
        }

        Type = type;
    }
}
