using System.Threading;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Common.Interfaces.Services;
using CleanArchTemplate.Domain.AuditTrails;
using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Infrastructure.Persistences.Models;
using CleanArchTemplate.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using static CleanArchTemplate.Shared.Constants.Permission.Permissions;

namespace CleanArchTemplate.Infrastructure.Persistence.Database.Interceptors;


public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTime;

    public AuditableEntitySaveChangesInterceptor(
        ICurrentUserService currentUserService,
        IDateTimeService dateTime)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var auditEntries = OnBeforeSaveChanges(eventData.Context!);
        UpdateEntities(eventData.Context);
        var response = base.SavingChanges(eventData, result);
        OnAfterSaveChanges(eventData.Context!, auditEntries,CancellationToken.None)
            .GetAwaiter()
            .GetResult();
        return response;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var auditEntries=OnBeforeSaveChanges(eventData.Context!);
        UpdateEntities(eventData.Context);
        var response= await base.SavingChangesAsync(eventData, result, cancellationToken);
        await OnAfterSaveChanges(eventData.Context!,auditEntries,cancellationToken);
        return response;
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;
        foreach (var entry in context.ChangeTracker.Entries<ICreatable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.ToCreate(_dateTime.NowUtc, _currentUserService.UserId ?? "");
            }
        }
        foreach (var entry in context.ChangeTracker.Entries<IModifiable>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.ToModify(_dateTime.NowUtc, _currentUserService.UserId ?? "");
            }
        }
        foreach (var entry in context.ChangeTracker.Entries<IDeletable>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.Entity.ToDelete(_dateTime.NowUtc, _currentUserService.UserId ?? "");
                entry.State = EntityState.Modified;
            }
        }
    }
    private List<AuditEntry> OnBeforeSaveChanges(DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditEntry>();
        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is Audit || entry.Entity is INotTracking || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var auditEntry = new AuditEntry(entry)
            {
                DateTime =_dateTime.NowUtc ,
                TableName = entry.Entity.GetType().Name,
                UserId = _currentUserService.UserId
                
            };
            auditEntries.Add(auditEntry);
            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    auditEntry.TemporaryProperties.Add(property);
                    continue;
                }

                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue!;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditType = AuditType.Create;
                        auditEntry.NewValues[propertyName] = property.CurrentValue!;
                        break;

                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditType.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue!;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.AuditType = AuditType.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue!;
                        }
                        break;
                }
            }
        }
        foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
        {
            context.Set<Audit>()
                .Add(auditEntry.ToAudit());
        }
        return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
    }

    private Task OnAfterSaveChanges(DbContext context, List<AuditEntry> auditEntries, CancellationToken cancellationToken = new())
    {
        ArgumentNullException.ThrowIfNull(context);
        if (auditEntries == null || auditEntries.Count == 0)
            return Task.CompletedTask;

        foreach (var auditEntry in auditEntries)
        {
            foreach (var prop in auditEntry.TemporaryProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue!;
                }
                else
                {
                    auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue!;
                }
            }
            context.Set<Audit>().Add(auditEntry.ToAudit());
        }
        return context.SaveChangesAsync(cancellationToken);
    }
}


public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified || r.TargetEntry.State == EntityState.Deleted));
}
