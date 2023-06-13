namespace CleanArchTemplate.Shared.Responses.Vehicles;

public class BrandResponse {
    public BrandResponse()
    {

    }
    public BrandResponse(Guid id, string name, string? logo)
    {
        Id = id;
        Name = name;
        Logo = logo;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Logo { get; set; }

}
