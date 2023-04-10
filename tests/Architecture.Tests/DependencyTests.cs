using System;
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
        var domainAssambly = typeof(IDomainAssambly).Assembly;
        var applicationAssambly = typeof(IApplicationAssambly).Assembly;
        domainAssambly.Should()
            .NotReference(applicationAssambly);
    }
    [Fact]
    public void Application_ShouldDependOnDomain()
    {
        var domainAssambly = typeof(IDomainAssambly).Assembly;
        var applicationAssambly = typeof(IApplicationAssambly).Assembly;
        applicationAssambly.Should().Reference(domainAssambly);
    }
    [Fact]
    public void WebApi_ShouldNotDependOnDomain()
    {
        var domainAssambly = typeof(IDomainAssambly).Assembly;
        var serverAssambly = typeof(IServerAssambly).Assembly;
        serverAssambly.Should().NotReference(domainAssambly);
    }
    [Fact]
    public void WebApi_ShouldNotDependOnInfrastructure()
    {
        var domainAssambly = typeof(IInfrastructureAssambly).Assembly;
        var serverAssambly = typeof(IServerAssambly).Assembly;
        serverAssambly.Should().NotReference(domainAssambly);
    }
}

