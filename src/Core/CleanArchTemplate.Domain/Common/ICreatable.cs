namespace CleanArchTemplate.Domain.Common;

public interface ICreatable
{
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public virtual void ToCreate(DateTimeOffset at, string by)
    {
        CreatedAt = at;
        CreatedBy = by;
    }
}
