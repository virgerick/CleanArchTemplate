namespace CleanArchTemplate.Shared.Responses.Vehicles;

public class ModelResponse
{
    public ModelResponse()
    {

    }
    public ModelResponse(Guid id, string name, int year, Guid brandId, Guid typeId)
    {
        Id = id;
        Name = name;
        Year = year;
        BrandId = brandId;
        TypeId = typeId;
    }

    public Guid Id { get; set; }
    public string Name { get;  set; }
    public int Year { get;  set; }
    public Guid BrandId { get;  set; }
    public Guid TypeId { get;  set; }
}
public record  ModelDefaultResponse(ModelResponse[] Models, BrandResponse[] Brands,IdNameResponse[] VehicleTypes);