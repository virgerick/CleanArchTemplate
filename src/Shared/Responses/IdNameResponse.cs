

namespace CleanArchTemplate.Shared.Responses;

public record IdNameResponse<T>(T Id,string Name) {
};

public class IdNameResponse {
    public IdNameResponse(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    public IdNameResponse()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
    }
    public Guid Id { get; set; }
    public string Name{get;set;}
    public static IdNameResponse Empty() => new IdNameResponse(Guid.Empty, string.Empty);
}
