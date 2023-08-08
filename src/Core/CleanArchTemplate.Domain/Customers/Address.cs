namespace CleanArchTemplate.Domain.Customers;

public record Address(string Street, string City, string State, string ZipCode)
{
    public static readonly Address Empty = new(String.Empty, String.Empty, string.Empty, string.Empty);
};
