namespace CleanArchTemplate.Domain.Invoices;

public abstract record ServiceStatus
{
    public static string Active = nameof(Active);
    public static string Inactive = nameof(Inactive);
    public static string Scheduled = nameof(Scheduled);
    public static string InProgress = nameof(InProgress);
    public static string Completed = nameof(Completed);

    public string Status { get; }

    public ServiceStatus(string status)
    {
        if (status != Scheduled && status != InProgress && status != Completed)
        {
            throw new ArgumentException($"Invalid service status: {status}");
        }

        Status = status;
    }
}
