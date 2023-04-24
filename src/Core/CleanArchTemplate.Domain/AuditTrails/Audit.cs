using System;
namespace CleanArchTemplate.Domain.AuditTrails;

public class Audit : BaseEntity<Guid>
{
    public required string UserId { get; set; }
    public required string Type { get; set; }
    public required string TableName { get; set; }
    public required DateTimeOffset DateTime { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? AffectedColumns { get; set; }
    public string? PrimaryKey { get; set; }
}
