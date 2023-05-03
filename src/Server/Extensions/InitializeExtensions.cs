using CleanArchTemplate.Application.Common.Interfaces.Services;

namespace CleanArchTemplate.Server.Extensions
{
public static class InitializeExtensions
{

    public static void UseInitialize(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

            var initializers = serviceScope.ServiceProvider.GetServices<IDatabaseSeeder>();

            foreach (var initializer in initializers)
            {
                initializer.Initialize();
            }
    }
}
}