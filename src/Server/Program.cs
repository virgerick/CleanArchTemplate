
using CleanArchTemplate.Infrastructure;
using CleanArchTemplate.Application;
using CleanArchTemplate.Server.Extensions;

using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using CleanArchTemplate.Application.Common.Interfaces.Common;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services
    .AddGlobalization()
    .AddSharedInfrastructure(configuration)
    .AddCurrentUserService()
    .AddInterceptors()
    .AddApplicationMediatR()
    .AddDatabase(configuration)
    .AddIdentity()
    .AddSwaggerGen();

var app = builder.Build();

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
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseGlobalization();
app.UseRouting();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

