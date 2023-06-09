﻿namespace CleanArchTemplate.Infrastructure;
using CleanArchTemplate.Infrastructure.Persistence.Database.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection Infrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
    public static IServiceCollection AddInterceptors(this IServiceCollection services)
    {
        return services
            .AddScoped<AuditableEntitySaveChangesInterceptor>();
    }
   
}

