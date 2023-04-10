namespace CleanArchTemplate.Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection Infrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}

