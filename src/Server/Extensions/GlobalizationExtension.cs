using CleanArchTemplate.Shared.Constants.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

using System.Globalization;

namespace CleanArchTemplate.Server.Extensions;

public static class GlobalizationExtension
{
    public static IServiceCollection AddGlobalization(this IServiceCollection services)
    {
        const string defaultCulture = "en-US";
        var supportedCultures = LocalizationConstants.SupportedLanguages
            .Select(x=>new CultureInfo(x.Code))
            .ToList();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(defaultCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        services.AddLocalization(option => option.ResourcesPath = "Resources");
        return services;
    }

    public static IApplicationBuilder UseGlobalization(
    this WebApplication app)
    {
        app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
        return app;
    }
}
