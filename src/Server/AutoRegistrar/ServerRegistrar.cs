using CleanArchTemplate.Application.Common.Interfaces.Common;
using CleanArchTemplate.Application;
using CleanArchTemplate.Infrastructure;
using CleanArchTemplate.Server.Endpoints.Identity.Token;
using CleanArchTemplate.Server.Extensions;

namespace CleanArchTemplate.Server.AutoRegistrar;

public class ServerRegistrar : IRegistrarApplicationBuilder, IRegistrarApplication
{
    public void Add(object Builder)
    {
        if (Builder is WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services
                        .AddSharedInfrastructure(builder.Configuration)
                        .AddCurrentUserService()
                        .AddInterceptors()
                        .AddApplicationMediatR()
                        .AddDatabase(builder.Configuration)
                        .AddIdentity()
                        .AddApplicationServices()
                        .AddSwaggerGen();
        }
    }

    public void Use(object App)
    {
        if (App is WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseRouting();
            app.MapRazorPages();
            app.MapControllers();
            app.UseInitialize();
            app.MapFallbackToFile("index.html");
        }
    }
}
