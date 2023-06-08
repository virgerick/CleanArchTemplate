using System;
using System.Reflection;
using Blazored.LocalStorage;
using CleanArchTemplate.Client.Authentication;
using CleanArchTemplate.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


namespace CleanArchTemplate.Client.Extensions;

public static class ComponentExtension
{
	
}
public static class ArrayExtensions
{
    private static readonly Random Random = new Random();

    public static T GetRandomElement<T>(this T[] array)
    {
        if (array == null || array.Length == 0)
        {
            throw new ArgumentException("El array no puede ser null y debe tener al menos un elemento.");
        }

        var index = Random.Next(array.Length);
        return array[index];
    }
}
public static class EnumExtensions
{
    public static T GetRandomValue<T>(this T value) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        return  values.GetRandomElement();
    }
}

public static class AuthenticationExtentions {

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
            .AddScoped<AuthenticationStateProvider, BlazorStateProvider>();

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