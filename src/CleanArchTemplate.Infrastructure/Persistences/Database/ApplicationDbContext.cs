using System;
using System.Reflection;
using CleanArchTemplate.Domain;
using System.Reflection.Emit;
using CleanArchTemplate.Domain.Identity;
using CleanArchTemplate.Infrastructure.Persistences.Database.Interceptors;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CleanArchTemplate.Application.Common.Interfaces;

namespace CleanArchTemplate.Infrastructure.Persistences.Database;

public class ApplicationDbContext :
    IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim, IdentityUserToken<string>>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    protected ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var entitiesAssembly = typeof(IEntity).Assembly;
        builder.RegisterAllEntities<IEntity>(entitiesAssembly);
        builder.ApplyConfigurationsFromAssembly(typeof(IEntity).Assembly);
        builder.AddPluralizingTableNameConvention();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);
        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> ExecuteSqlRawAsync(string query, CancellationToken cancellationToken=default)
    {
        var result = await base.Database.ExecuteSqlRawAsync(query, cancellationToken);
        return result;
    }

    
}

