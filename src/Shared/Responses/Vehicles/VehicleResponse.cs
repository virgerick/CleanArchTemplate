namespace CleanArchTemplate.Shared.Responses.Vehicles;
public class VehicleResponse
{
    public VehicleResponse()
    {

    }

    public VehicleResponse(Guid id, string plateNumber, string color, string status, Guid modelId, string model, string brand, int? year, bool deleted)
    {
        Id = id;
        PlateNumber = plateNumber;
        Color = color;
        Status = status;
        ModelId = modelId;
        Model = model;
        Brand = brand;
        Year = year;
        Deleted = deleted;
    }

    public Guid Id { get; set; }
    public string PlateNumber { get; set; }
    public string Color { get; set; } = "";
    public string Status { get; set; }
    public Guid ModelId { get; set; }
    public string Model { get; set; }
    public string Brand { get; set; }
    public int? Year { get; set; }
    public bool Deleted { get; set; }

}
public record  VehicleDefaultResponse(VehicleResponse[] Vehicles, ModelResponse[] Models);