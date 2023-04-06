using System;
namespace CleanArchTemplate.Domain.Common;

public interface IAuditableEntity : IEntity, ICreatable, IModifiable{}
public interface IAuditableEntity<TKey> : IEntity<TKey>, IEntity, ICreatable, IModifiable{}
public interface IAuditableRootEntity<TKey> : IAuditableEntity<TKey>, IDeletable{}
public interface IAuditableRootEntity : IAuditableEntity<int>, IDeletable{}
public abstract class AuditableEntity : AuditableEntity<int>{}
public abstract class AuditableEntity<TKey> : IAuditableEntity<TKey>
{
    public TKey Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string ModifiedBy { get; set; }
}

public abstract class AuditableRootEntity : AuditableRootEntity<int>{}

public abstract class AuditableRootEntity<TKey> : AuditableEntity<TKey>, IAuditableRootEntity<TKey>
{
    public bool Deleted { get ; set ; }
    public string? DeletedBy { get ; set ; }
    public DateTime? DeletedAt { get ; set ; }
}