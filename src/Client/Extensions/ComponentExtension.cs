
using Microsoft.AspNetCore.Components;

namespace CleanArchTemplate.Client.Extensions;

public static class NavigateManagerExtensions
{
    public static void NavigateToLogin(this NavigationManager manager, string loginPath)
    {
        manager.NavigateTo(loginPath, new NavigationOptions
        {
            HistoryEntryState = manager.Uri
        });
    }
    public static void NavigateToBackward(this NavigationManager manager)
    {
        manager.NavigateTo(manager.HistoryEntryState ?? "/");
    }
}