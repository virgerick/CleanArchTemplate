namespace CleanArchTemplate.Domain.Invoices;

public abstract record VehicleStatus
{
    public const string Available = nameof(Available);
    public const string Maintenance = nameof(Maintenance);
    public const string OutOfService = nameof(OutOfService);

    public string Status { get; }

    public VehicleStatus(string status)
    {
       

        Status = status;
    }
    public static IEnumerable<VehicleStatus> Supported
    {
        get
        {
            yield return new AvailableStatus();
            yield return new MaintenanceStatus();
            yield return new OutOfServiceStatus();
        }
    }

    public static VehicleStatus Create(string status)
    {
        return Supported.SingleOrDefault(x=>x.Status==status) ?? throw new ArgumentException($"Invalid vehicle status: {status}");
    }
    public static implicit operator string(VehicleStatus status) => status.Status;
    public static implicit operator VehicleStatus(string status) => Create(status);

}
public record AvailableStatus() : VehicleStatus(VehicleStatus.Available);
public record MaintenanceStatus() : VehicleStatus(VehicleStatus.Maintenance);
public record OutOfServiceStatus() : VehicleStatus(VehicleStatus.OutOfService);
