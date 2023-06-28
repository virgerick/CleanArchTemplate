using System.Reflection;
using Blazored.LocalStorage;
using CleanArchTemplate.Client.Authentication;
using CleanArchTemplate.Client.Services.Crypto;
using CleanArchTemplate.Client.Services.LocalStorages;
using CleanArchTemplate.Client.Storages;
using CleanArchTemplate.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanArchTemplate.Client.Extensions;

public static class ServiceProviderExtentions
{

    public static WebAssemblyHostBuilder AddServiceProviders(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services
            .AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            })
            .AddAuthorizationCore(RegisterPermissionClaims)
            .AddBlazoredLocalStorage()
            //.AddScoped<ClientPreferenceManager>()
            .AddScoped<BlazorStateProvider>()
            .AddScoped<IAuthenticationManager, AuthenticationManager>()
            .AddScoped<AuthenticationStateProvider, BlazorStateProvider>()
            .AddTransient<AuthenticationHeaderHandler>()
            .AddSubtleCrypto(opt =>
            opt.Key = "FwCvfdOhoJm5sB/XBMCoM05CM4J/3VC8P0PROIgFNJ/+mPJfzv0ivfMG2a156sd14@#@#$@>NJKBiknqwhodmnMKLNDjklanojHifUlhdXkdsljf")
            .AddScoped<IEncrytedLocalStorageService, EncrytedLocalStorageService>()
            .AddScoped<MainStorage>();

        return builder;
    }
    private static void RegisterPermissionClaims(AuthorizationOptions options)
    {
        foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
        {
            var propertyValue = prop.GetValue(null);
            if (propertyValue is not null)
            {
                options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString()));
            }
        }
    }
}