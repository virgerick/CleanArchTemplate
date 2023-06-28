using System.Net.Http.Headers;
using CleanArchTemplate.Client.Authentication;
using CleanArchTemplate.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

namespace CleanArchTemplate.Client.Extensions;

public static class RefixExtension
{
    public static WebAssemblyHostBuilder ConfigureRefixServices(this WebAssemblyHostBuilder builder)
    {
        builder.ConfigureRefitClientFor<IBrandApiService>();
        builder.ConfigureRefitClientFor<ICustomerAPIService>();
        builder.ConfigureRefitClientFor<IDriverAPIService>();
        builder.ConfigureRefitClientFor<IModelAPIService>();
        builder.ConfigureRefitClientFor<ITokenApiService>();
        builder.ConfigureRefitClientFor<IRouteAPIService>();
        builder.ConfigureRefitClientFor<IServiceApiService>();
        builder.ConfigureRefitClientFor<IVehicleAPIService>();
        builder.ConfigureRefitClientFor<IVehicleTypeAPIService>();
        //you could add Polly here to handle HTTP 429 / HTTP 503 etc
        return builder;
    }

    static WebAssemblyHostBuilder ConfigureRefitClientFor<T>(this WebAssemblyHostBuilder builder)
        where T : class
    {
        /*
        services.AddTransient<ITenantProvider, TenantProvider>();
        services.AddTransient<IAuthTokenStore, AuthTokenStore>();
        services.AddTransient<AuthHeaderHandler>();
        */
        //this will add our refit api implementation with an HttpClient
        //that is configured to add auth headers to all requests

        //note: AddRefitClient<T> requires a reference to Refit.HttpClientFactory
        //note: the order of delegating handlers is important and they run in the order they are added!

        builder.Services
            .AddRefitClient<T>()
            .AddHttpMessageHandler<AuthenticationHeaderHandler>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
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
