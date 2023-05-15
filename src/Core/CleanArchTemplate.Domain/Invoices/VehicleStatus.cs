namespace CleanArchTemplate.Domain.Invoices;

public abstract record VehicleStatus
{
    public static string Available = nameof(AvailableStatus);
    public static string Maintenance = nameof(MaintenanceStatus);
    public static string OutOfService = nameof(OutOfServiceStatus);

    public string Status { get; }

    public VehicleStatus(string status)
    {
        if (status != Available && status != Maintenance && status != OutOfService)
        {
            throw new ArgumentException($"Invalid vehicle status: {status}");
        }

        Status = status;
    }
}
