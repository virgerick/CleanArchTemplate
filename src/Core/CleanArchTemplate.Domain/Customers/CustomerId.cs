namespace CleanArchTemplate.Domain.Invoices;

public record CustomerId(int Value)
{
    public static CustomerId Create(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentException("Customer ID must be a positive integer.", nameof(value));
        }

        return new CustomerId(value);
    }
}
