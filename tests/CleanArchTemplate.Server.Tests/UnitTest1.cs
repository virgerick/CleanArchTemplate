using CleanArchTemplate.Infrastructure;

using FluentAssertions;

namespace CleanArchTemplate.Server.Tests;

public class ServerTest
{
    [Fact]
    public void Server_ShouldNotDependOnInfrastructure()
    {
        var infrastructureAssambly = typeof(IInfrastructureAssemblyMarkup).Assembly;
        var serverAssambly = typeof(IServerAssemblyMarkup).Assembly;
        serverAssambly
            .Should()
            .NotReference(infrastructureAssambly)
            ;
    }
}