using CleanArchTemplate.Abstraction.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace CleanArchTemplate.Abstraction.Extensions
{
    internal static class HostBuilderExtensions
    {
        internal static IHostBuilder UseSerilog(this IApplicationBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            SerilogHostBuilderExtensions.UseSerilog(builder);
            return builder;
        }
    }
}