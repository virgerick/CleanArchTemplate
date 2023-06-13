namespace CleanArchTemplate.Shared.Responses.Vehicles;
public class VehicleResponse {
    public VehicleResponse()
    {

    }
    public VehicleResponse(Guid id, string plateNumber, string brand, string model, string type, string status,bool deleted)
    {
        Id = id;
        PlateNumber = plateNumber;
        Brand = brand;
        Model = model;
        Type = type;
        Status = status;
        Deleted = deleted;
    }

   public Guid Id{get;set;}
   public string PlateNumber{get;set;}
   public string Brand{get;set;}
   public string Model{get;set;}
   public string Type{get;set;}
   public string Status { get; set; }
   public bool Deleted { get; set; }

}
