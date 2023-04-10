namespace CleanArchTemplate.Domain.Common;

public interface IDeletable
{
    public bool Deleted { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
}
