namespace CleanArchTemplate.Domain.Common;

public interface IModifiable
{
   public DateTime? ModifiedAt { get; set; }
   public string? ModifiedBy { get; set; }
   public virtual void ToModify(DateTime at,string by)
   {
        ModifiedAt = at;
        ModifiedBy = by;
   }
}
