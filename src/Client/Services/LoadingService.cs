namespace CleanArchTemplate.Client.Services;

public class LoadingService
{
    public event Action OnChange=default!;
    private bool isLoading;
    private readonly ILogger<LoadingService> _logger;
    public static LoadingService Instance { get; private set; }
    public bool IsLoading
    {
        get { return isLoading; }
        set
        {
            isLoading = value;
            NotifyStateChanged();
        }
    }

    public LoadingService(ILogger<LoadingService> logger)
    {
       
        _logger = logger;
        Instance = this;
    }

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
    public void Show() {
        IsLoading = true;
    }
    
    public void Hide() {
        IsLoading = false;
    }

}
