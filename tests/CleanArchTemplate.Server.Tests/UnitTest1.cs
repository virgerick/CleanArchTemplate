using FluentAssertions;

namespace CleanArchTemplate.Server.Tests;

public class ServerTest
{
    [Fact]
    public void Server_ShouldNotDependOnInfrastructure()
    {
        var infrastructureAssambly = typeof(IInfrastructureAssambly).Assembly;
        var serverAssambly = typeof(IServerAssembly).Assembly;
        serverAssambly
            .Should()
            .Reference(infrastructureAssambly);
    }
}