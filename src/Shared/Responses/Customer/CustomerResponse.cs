using System.Net;

namespace CleanArchTemplate.Shared.Responses.Customers;
public class CustomerResponse
{
    public CustomerResponse()
    {

    }
    public CustomerResponse(Guid id, string name, string email, bool deleted, AddressResponse address)
    {
        Id = id;
        Name = name;
        Email = email;
        Deleted = deleted;
        Address = address;
    }
    public Guid Id { get; set; }
    public string Name { get;  set; }
    public string Email { get;  set; }
    private bool Deleted { get; set; }
    public AddressResponse Address { get;  set; }

}
