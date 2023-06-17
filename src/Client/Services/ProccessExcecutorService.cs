
using Radzen;

namespace CleanArchTemplate.Client.Services;

public class ProcessExecutorService {
    private readonly ILogger<ProcessExecutorService> _logger;
    private readonly LoadingService _loadingService;
    private readonly DialogService _dialogService;
    private readonly NotificationService _notificationService;
    public ProcessExecutorService(ILogger<ProcessExecutorService> logger,LoadingService loadingService,DialogService dialogService,NotificationService notificationService)
    {
        this._logger = logger;
        _loadingService = loadingService;
       _dialogService = dialogService;
       _notificationService = notificationService;
    }
    public async Task Run(Func<Task> action,Action<Exception>? onCatch = null
    ,Action? onFinally=null!) {

        try
        {
            _loadingService.Show();
            await action.Invoke();    
        }
        catch (Exception ex)
        {
           HandlerException(ex, onCatch);
        }
        finally {
            _loadingService.Hide();
            onFinally?.Invoke();
        }
    }
    private void HandlerException(Exception exception, Action<Exception>? onCatch) {
        _logger.LogError(exception.Message);
        _notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error,
            Detail = exception.Message });
        onCatch?.Invoke(exception);
    }
}