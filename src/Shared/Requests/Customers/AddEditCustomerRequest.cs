using System;
namespace CleanArchTemplate.Shared.Requests.Customers;

public record struct AddEditCustomerRequest
{
    public AddEditCustomerRequest()
    {

    }

    public AddEditCustomerRequest(string name, string email, AddressRequest address)
    {
        Name = name;
        Email = email;
        Address = address;
    }

    public string Name { get; set; }
    public string Email { get; set; }
    public AddressRequest Address { get; set; }
}


