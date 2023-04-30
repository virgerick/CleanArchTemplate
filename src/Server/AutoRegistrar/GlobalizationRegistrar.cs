using CleanArchTemplate.Application.Common.Interfaces.Common;
using CleanArchTemplate.Shared.Constants.Localization;

using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

using System.Globalization;

namespace CleanArchTemplate.Server.AutoRegistrar;

public class GlobalizationRegistrar : IRegistrarApplication, IRegistrarApplicationBuilder
{
    public void Add(object Builder)
    {
        if (Builder is WebApplicationBuilder builder)
        {
            const string defaultCulture = "en-US";
            var supportedCultures = LocalizationConstants.SupportedLanguages
                .Select(x => new CultureInfo(x.Code))
                .ToList();
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            })
            .AddLocalization(option => option.ResourcesPath = "Resources");
        }
    }

    public void Use(object App)
    {
        if (App is WebApplication app)
        {
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
        }
    }
}
