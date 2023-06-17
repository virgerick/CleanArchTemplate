using Microsoft.JSInterop;
using Microsoft.Extensions.Options;

namespace CleanArchTemplate.Client.Services.Crypto;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddSubtleCrypto(this IServiceCollection services, Action<CryptoOptions> setup = null!)
    {
        if (setup != null)
            services.Configure(setup);


        services.AddScoped<ICryptoService, CryptoService>(
               (ctx) =>{

                   var options = ctx.GetRequiredService<IOptions<CryptoOptions>>();
                   var jsRuntime = ctx.GetService<IJSRuntime>();
                   var key = options.Value.Key;

                   return new CryptoService(jsRuntime!, options.Value);
               });
        return services;
    }
}
