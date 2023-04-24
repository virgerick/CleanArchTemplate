namespace CleanArchTemplate.Domain.Common;

public interface IDeletable
{
    public bool Deleted { get; set; }
    public string? DeletedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public virtual void ToDelete(DateTimeOffset at, string by)
    {
        Deleted = true;
        DeletedAt = at;
        DeletedBy = by;
    }
    public virtual void ToRestore()
    {
        if (Deleted)
        {
            throw new InvalidOperationException("Invalid Operation this record is already deleted");
        }
        Deleted = false;
    }
}
