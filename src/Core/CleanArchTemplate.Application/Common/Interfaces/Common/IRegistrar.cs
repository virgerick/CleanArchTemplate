using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchTemplate.Application.Common.Interfaces.Common;

public interface IRegistrar
{
}
public interface IRegistrarServices:IRegistrar
{
    public void AddService(IServiceCollection services, IConfiguration configuration);
}
public interface IRegistrarApplicationBuilder : IRegistrar
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Builder">It's a ApplicationBuilder or A WebApplicationBuilder</param>
    public void Add(object Builder);   
}
public interface IRegistrarApplication : IRegistrar
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="App">It's a Application or a WebApplication</param>
    public void Use(object App);
}
