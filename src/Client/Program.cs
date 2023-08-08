using CleanArchTemplate.Client;
using CleanArchTemplate.Client.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.AddServiceProviders();
builder.ConfigureRefitServices();
builder.ConfigureRadzenServices();

await builder.Build().RunAsync();

