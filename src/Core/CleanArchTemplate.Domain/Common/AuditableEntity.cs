namespace CleanArchTemplate.Domain.Common;
public interface IAuditableEntity : IEntity, ICreatable, IModifiable { }
public interface IAuditableEntity<TKey> : IEntity<TKey>, IEntity, ICreatable, IModifiable { }
public interface IAuditableRootEntity<TKey> : IAuditableEntity<TKey>, IDeletable { }
public interface IAuditableRootEntity : IAuditableEntity<int>, IDeletable { }

public abstract class AuditableEntity : AuditableEntity<int> { }
public abstract class AuditableEntity<TKey> :BaseEntity<TKey>, IAuditableEntity<TKey>
{
    public TKey Id { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
}

public abstract class AuditableRootEntity : AuditableRootEntity<int> { }

public abstract class AuditableRootEntity<TKey> : AuditableEntity<TKey>, IAuditableRootEntity<TKey>
{
    public bool Deleted { get; set; }
    public string? DeletedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}