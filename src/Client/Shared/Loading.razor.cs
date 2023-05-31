
using Microsoft.AspNetCore.Components;
using CleanArchTemplate.Client.Services;
using CleanArchTemplate.Shared.Enums;
using CleanArchTemplate.Client.Shared.Components;
using CleanArchTemplate.Client.Extensions;

namespace CleanArchTemplate.Client.Shared;

public partial class Loading
{
    /*LoaderType type = LoaderType.Clock;
    RenderFragment Loader => type switch
    {
        LoaderType.Clock => new RenderFragment(builder => {
            builder.OpenComponent(0, typeof(ClockLoader));
            builder.CloseComponent();
        }),
        LoaderType.Hamster=>new RenderFragment(builder => {
            builder.OpenComponent(0, typeof(HamsterLoader));
            builder.CloseComponent();
        }),
        LoaderType.HoneyComb=>new RenderFragment(builder => {
            builder.OpenComponent(0, typeof(HoneyCombLoader));
            builder.CloseComponent();
        }),
    };*/

    protected override Task OnInitializedAsync()
    {
        LoadingService.OnChange += Handler;
        return base.OnInitializedAsync();
    }
    public void Handler()
    {
        //type=type.GetRandomValue();
        StateHasChanged();
    }
}
