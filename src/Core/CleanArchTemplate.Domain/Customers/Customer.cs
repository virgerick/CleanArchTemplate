

using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Contracts;
using CleanArchTemplate.Domain.Services;
using OneOf;

namespace CleanArchTemplate.Domain.Customers;
public record struct CustomerId(Guid Value);
public class Customer:AuditableRootEntity<CustomerId>
{
    private List<Contract> _contracts = new();
    private List<Service> _services = new();
    public string Name { get; private set; }
    public string Email { get; private set; }
    public Address Address { get; private set; }
    public IReadOnlyList<Contract> Contracts => _contracts;
    public IReadOnlyList<Service> Services => _services;

    protected Customer() { } // Constructor protegido para EF Core

    public static OneOf<Customer,Exception> Create(string name, string email, Address address)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new ArgumentException("Customer name must not be empty.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return new ArgumentException("Email cannot be empty.", nameof(email));
        }

        if (!email.Contains('@'))
        {
            return new ArgumentException("Email is not valid.", nameof(email));
        }

        return new Customer { Name = name, Email = email, Address = address };
    }

    public OneOf<bool,Exception> Update(string name, string email, Address address)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new ArgumentException("Customer name must not be empty.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return new ArgumentException("Email cannot be empty.", nameof(email));
        }

        if (!email.Contains('@'))
        {
            return new ArgumentException("Email is not valid.", nameof(email));
        }

        var changed = false;
        if (Name != name)
        {
            Name = name;
            changed = true;
        }
        if (Email != email)
        {
            Email = email;
            changed = true;
        }
        if (Address != address)
        {
            Address = address;
            changed = true;
        }
        return changed;
    }


}
