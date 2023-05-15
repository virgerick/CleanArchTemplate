namespace CleanArchTemplate.Domain.Invoices;

public record ContractId(int Value)
{
    public static ContractId Create(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentException("Contract ID must be a positive integer.", nameof(value));
        }

        return new ContractId(value);
    }
}
