namespace CleanArchTemplate.Domain.Common;

public interface IModifiable
{
    public DateTimeOffset? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public virtual void ToModify(DateTimeOffset at, string by)
    {
        ModifiedAt = at;
        ModifiedBy = by;
    }
}
