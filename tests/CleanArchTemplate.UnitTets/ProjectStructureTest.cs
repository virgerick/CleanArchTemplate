using CleanArchTemplate.Application;
using CleanArchTemplate.Domain;
using FluentAssertions;

namespace CleanArchTemplate.UnitTets;

public class ProjectStructureTest
{
    [Fact]
    public void DomainShouldNotDependOnApplication() {
        var domainAssambly = typeof(IDomainAssemblyMarkup).Assembly;
        domainAssambly
            .Should()
            .NotReference(typeof(IApplicationAssemblyMarkup).Assembly);
            
    }
    [Fact]
    public void ApplicationShouldDependOnDomin() {
        var domainAssembly = typeof(IDomainAssemblyMarkup).Assembly;
        var applicationAssembly = typeof(IApplicationAssemblyMarkup).Assembly;
        applicationAssembly
            .Should()
            .Reference(domainAssembly);
            
    }
}

