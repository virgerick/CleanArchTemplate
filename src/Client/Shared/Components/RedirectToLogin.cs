
using Microsoft.AspNetCore.Components;

namespace CleanArchTemplate.Client.Shared.Components;

public partial class RedirectToLogin:ComponentBase
{
    protected  void OnInitialized()
    {
        _navigationManager.NavigateTo("/login");
        base.OnInitialized();
    }
}

