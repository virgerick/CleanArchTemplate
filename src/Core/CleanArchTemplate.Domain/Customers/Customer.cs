namespace CleanArchTemplate.Domain.Invoices;

public class Customer
{
    public CustomerId Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public Address Address { get; private set; }
    public ICollection<Contract> Contracts { get; private set; }

    protected Customer() { } // Constructor protegido para EF Core

    public static Customer Create(string name, string email, Address address)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Customer name must not be empty.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be empty.", nameof(email));
        }

        if (!email.Contains('@'))
        {
            throw new ArgumentException("Email is not valid.", nameof(email));
        }

        return new Customer { Name = name, Email = email, Address = address };
    }

    public void UpdateEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail))
        {
            throw new ArgumentException("Email cannot be empty.", nameof(newEmail));
        }

        if (!newEmail.Contains('@'))
        {
            throw new ArgumentException("Email is not valid.", nameof(newEmail));
        }

        Email = newEmail;
    }
}
