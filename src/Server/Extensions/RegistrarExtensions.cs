using CleanArchTemplate.Application.Common.Interfaces.Common;

namespace CleanArchTemplate.Server.Extensions;

/// <summary>
/// Extension for registrar 
/// </summary>
public static class RegistrarExtensions
{
    /// <summary>
    /// Registrar all Implementation o IRegistrarApplicationBuilder on an Assembly
    /// </summary>
    /// <typeparam name="T">Represent the Assembly to scans</typeparam>
    /// <param name="builder">It is the web application builder instance</param>
    public static void AddRegistrars<T>(this WebApplicationBuilder builder)
    {
        Type scanningType= typeof(T);
        var servicesRegistrar=GetRegistrars<IRegistrarServices>(scanningType);
        foreach (var registrar in servicesRegistrar)
        {
            registrar.AddService(builder.Services, builder.Configuration);
        }
        var builderRegistrars = GetRegistrars<IRegistrarApplicationBuilder>(scanningType);
        foreach (var registrar in builderRegistrars)
        {
            registrar.Add(builder);
        }
    }
    /// <summary>
    /// Registrar all implementation of a IRegistrarApplication
    /// </summary>
    /// <typeparam name="T">Represent the Assembly to scans</typeparam>
    /// <param name="app">It is the web application instance</param>
    public static void AddRegistrars<T>(this WebApplication app) where T : class
    { 
        Type scanningType=typeof(T);
        var registrars = GetRegistrars<IRegistrarApplication>(scanningType);
        foreach (var registrar in registrars)
        {
            registrar.Use(app);
        }
    }
   private static IEnumerable<T> GetRegistrars<T>(Type scanningType) where T : IRegistrar
    {
        return scanningType.Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(T)) && !t.IsAbstract && !t.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<T>();
    }
}
