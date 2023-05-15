namespace CleanArchTemplate.Domain.Invoices;

public class Vehicle
{
    public VehicleId Id { get; private set; }
    public string PlateNumber { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public VehicleType Type { get; private set; }
    public VehicleStatus Status { get; private set; }
    public ICollection<Service> Services { get; private set; }

    protected Vehicle() { } // Constructor protegido para EF Core

    public static Vehicle Create(VehicleId id, string plateNumber, string brand, string model, VehicleType type)
    {
        if (string.IsNullOrWhiteSpace(plateNumber))
        {
            throw new ArgumentException("Plate number must not be empty.", nameof(plateNumber));
        }

        if (string.IsNullOrWhiteSpace(brand))
        {
            throw new ArgumentException("Brand must not be empty.", nameof(brand));
        }

        if (string.IsNullOrWhiteSpace(model))
        {
            throw new ArgumentException("Model must not be empty.", nameof(model));
        }

        return new Vehicle { Id = id, PlateNumber = plateNumber, Brand = brand, Model = model, Type = type, Status = new AvailableStatus() };
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
}
