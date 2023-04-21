namespace CleanArchTemplate.Application;

using MediatR.NotificationPublishers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
    public static IServiceCollection AddApplicationMediatR(this IServiceCollection services)
    {
        return services
            .AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<IApplicationAssemblyMarkup>();
                config.NotificationPublisher = new ForeachAwaitPublisher();
                //config.NotificationPublisher = new TaskWhenAllPublisher();
                
            });
    }
}