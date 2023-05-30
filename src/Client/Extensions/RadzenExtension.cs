using CleanArchTemplate.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

namespace CleanArchTemplate.Client.Extensions;

public static class RadzenExtension
{
    public static WebAssemblyHostBuilder ConfigureRadzenServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddScoped<TooltipService>();
        builder.Services.AddScoped<ContextMenuService>();
        //
        builder.Services.AddSingleton<LoadingService>();

        return builder;
    }
}
/*class AuthHeaderHandler : DelegatingHandler
{
    private readonly ITenantProvider tenantProvider;
    private readonly IAuthTokenStore authTokenStore;

    public AuthHeaderHandler(ITenantProvider tenantProvider, IAuthTokenStore authTokenStore)
    {
        this.tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(tenantProvider));
        this.authTokenStore = authTokenStore ?? throw new ArgumentNullException(nameof(authTokenStore));
        InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await authTokenStore.GetToken();

        //potentially refresh token here if it has expired etc.

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Headers.Add("X-Tenant-Id", tenantProvider.GetTenantId());

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}*/