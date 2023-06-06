namespace CleanArchTemplate.Domain.Services;

public abstract record ServiceStatus
{
    public static string Active = nameof(Active);
    public static string Inactive = nameof(Inactive);
    public static string Scheduled = nameof(Scheduled);
    public static string InProgress = nameof(InProgress);
    public static string Completed = nameof(Completed);

    public string Status { get; }

    private protected ServiceStatus(string status)
    {
        
        Status = status;
    }
    public static IEnumerable<ServiceStatus> Supported{
        get
        {
            yield return new ActiveStatus();
            yield return new InactiveStatus();
            yield return new InProgressStatus();
            yield return new ScheduledStatus();
            yield return new CompletedStatus();
        }
    }
    public static ServiceStatus Create(string status)
    {
        var found = Supported.SingleOrDefault(x => x.Status == status);
        if(found is null)throw new ArgumentException($"Invalid service status: {status}");
        return found;
    }
    public static implicit operator ServiceStatus(string status) => Create(status);
    public static implicit operator string(ServiceStatus status) => status.Status;
}
public record ActiveStatus() : ServiceStatus(ServiceStatus.Active);
public record InactiveStatus() : ServiceStatus(ServiceStatus.Inactive);
public record InProgressStatus() : ServiceStatus(ServiceStatus.InProgress);
public record ScheduledStatus() : ServiceStatus(ServiceStatus.Scheduled);
public record CompletedStatus() : ServiceStatus(ServiceStatus.Completed);