namespace CleanArchTemplate.Domain.Common;

public interface IDeletable
{
    public bool Deleted { get; set; }
    public string? DeletedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
public static class DeletableExtension{
    public static void ToDelete(this IDeletable deletable,DateTimeOffset at, string by)
    {
        deletable.Deleted = true;
        deletable.DeletedAt = at;
        deletable.DeletedBy = by;
    }
    public static void ToRestore(this IDeletable deletable)
    {
        if (!deletable.Deleted)
        {
            throw new InvalidOperationException("Invalid Operation this record is not deleted yet." );
        }
        deletable.Deleted = false;
    }
}