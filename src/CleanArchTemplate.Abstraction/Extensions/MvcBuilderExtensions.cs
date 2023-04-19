using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace CleanArchTemplate.Abstraction.Extensions
{
    internal static class MvcBuilderExtensions
    {
        internal static IMvcBuilder AddValidators(this IMvcBuilder builder)
        {
            builder.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AppConfiguration>());
            return builder;
        }

    }
}