using CleanArchTemplate.Server;
using CleanArchTemplate.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.AddRegistrars<IServerAssemblyMarkup>();
var app = builder.Build();
app.UseRegistrars<IServerAssemblyMarkup>();
app.Run();

