namespace CleanArchTemplate.Domain.Invoices;

public abstract record ContractType
{
    public static string Trip = nameof(Trip);
    public static string Monthly = nameof(Monthly);

    public string Type { get; }

    public ContractType(string type)
    {
        if (type != Trip && type != Monthly)
        {
            throw new ArgumentException($"Invalid contract type: {type}");
        }

        Type = type;
    }
}
