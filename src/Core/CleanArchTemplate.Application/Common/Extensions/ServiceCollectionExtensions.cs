using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace CleanArchTemplate.Application.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
           // services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(options=> {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                });
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

    }
}