namespace CleanArchTemplate.Domain.Common;

public interface ICreatable
{
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
}
