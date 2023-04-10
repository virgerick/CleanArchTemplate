using CleanArchTemplate.Application;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Infrastructure;
using CleanArchTemplate.Server;

using FluentAssertions;

namespace Architecture.Tests;

public class DependencyTests
{
    [Fact]
    public void Domain_ShouldNotDependOnApplication()
    {
        var domainAssambly = typeof(IDomainAssemblyMarkup).Assembly;
        var applicationAssambly = typeof(IApplicationAssemblyMarkup).Assembly;
        domainAssambly.Should()
            .NotReference(applicationAssambly);
    }
    [Fact]
    public void Application_ShouldDependOnDomain()
    {
        var domainAssambly = typeof(IDomainAssemblyMarkup).Assembly;
        var applicationAssambly = typeof(IApplicationAssemblyMarkup).Assembly;
        applicationAssambly.Should().Reference(domainAssambly);
    }
    [Fact]
    public void WebApi_ShouldNotDependOnDomain()
    {
        var domainAssambly = typeof(IDomainAssemblyMarkup).Assembly;
        var serverAssambly = typeof(IServerAssemblyMarkup).Assembly;
        serverAssambly.Should().NotReference(domainAssambly);
    }
    [Fact]
    public void WebApi_ShouldNotDependOnInfrastructure()
    {
        var infrastructureAssembly = typeof(IInfrastructureAssemblyMarkup).Assembly;
        var serverAssembly = typeof(IServerAssemblyMarkup).Assembly;
        serverAssembly.Should().NotReference(infrastructureAssembly);
    }
}

