
using Radzen;

namespace CleanArchTemplate.Client.Services;

public class ProccessExcecutorService {
    private readonly LoadingService _loadingService;
    private readonly DialogService _dialogService;
    private readonly NotificationService _notificationService;
    public ProccessExcecutorService(LoadingService loadingService,DialogService dialogService,NotificationService notificationService)
    {
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
        _notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error,
            Detail = exception.Message });
        onCatch?.Invoke(exception);
    }
}