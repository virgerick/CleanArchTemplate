namespace CleanArchTemplate.Domain.Contracts;

public abstract record ContractType
{
    public static string Trip = nameof(Trip);
    public static string Monthly = nameof(Monthly);

    public string Type { get; }

    public ContractType(string type)
    {
        Type = type;
    }
    public static IEnumerable<ContractType> Supported
    {
        get
        {
            yield return new TripContract();
            yield return new MonthlyContract();
        }
    }
    public static ContractType Create(string type)
    {
        var found = Supported.SingleOrDefault(x => x.Type == type);
        if(found is null ) throw new ArgumentException($"Invalid contract type: {type}");
        return found;

    }
}
public record TripContract() : ContractType(ContractType.Trip);
public record MonthlyContract() : ContractType(ContractType.Monthly);