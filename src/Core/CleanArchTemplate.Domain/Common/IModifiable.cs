namespace CleanArchTemplate.Domain.Common;

public interface IModifiable
{
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
   
}
