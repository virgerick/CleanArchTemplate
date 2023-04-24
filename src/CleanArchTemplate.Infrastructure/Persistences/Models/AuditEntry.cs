using System;
using CleanArchTemplate.Domain.AuditTrails;
using CleanArchTemplate.Shared.Enums;
using CleanArchTemplate.Shared.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanArchTemplate.Infrastructure.Persistences.Models;

public class AuditEntry
{
    public AuditEntry(EntityEntry entry)
    {
        Entry = entry;
    }
    public required DateTimeOffset DateTime { get; set; }
    public EntityEntry Entry { get; }
    public required string UserId { get; set; }
    public required string TableName { get; set; }
    public Dictionary<string, object> KeyValues { get; } = new();
    public Dictionary<string, object> OldValues { get; } = new();
    public Dictionary<string, object> NewValues { get; } = new();
    public List<PropertyEntry> TemporaryProperties { get; } = new();
    public AuditType AuditType { get; set; }
    public List<string> ChangedColumns { get; } = new();
    public bool HasTemporaryProperties => TemporaryProperties.Any();

    public Audit ToAudit()
    {
        var audit = new Audit
        {
            UserId = UserId,
            Type = AuditType.ToString(),
            TableName = TableName,
            DateTime =DateTime,
            PrimaryKey = KeyValues.ToJsonSerialize(),
            OldValues = OldValues.Count == 0 ? null : OldValues.ToJsonSerialize(),
            NewValues = NewValues.Count == 0 ? null : NewValues.ToJsonSerialize(),
            AffectedColumns = ChangedColumns.Count == 0 ? null : ChangedColumns.ToJsonSerialize()
        };
        return audit;
    }
}