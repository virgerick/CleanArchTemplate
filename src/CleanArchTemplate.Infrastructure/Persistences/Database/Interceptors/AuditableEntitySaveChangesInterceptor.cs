using System;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchTemplate.Infrastructure.Persistences.Database.Interceptors;


public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public AuditableEntitySaveChangesInterceptor(
        ICurrentUserService currentUserService,
        IDateTime dateTime)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<ICreatable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _currentUserService.UserId!;
                entry.Entity.CreatedAt = _dateTime.Now;
            }
        }
        foreach (var entry in context.ChangeTracker.Entries<IModifiable>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedBy = _currentUserService.UserId!;
                entry.Entity.ModifiedAt = _dateTime.Now;
            }
        }
        foreach (var entry in context.ChangeTracker.Entries<IDeletable>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.Entity.Deleted = true;
                entry.Entity.DeletedBy = _currentUserService.UserId!;
                entry.Entity.DeletedAt = _dateTime.Now;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
