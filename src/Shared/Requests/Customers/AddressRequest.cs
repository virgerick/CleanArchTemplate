namespace CleanArchTemplate.Shared.Requests.Customers;

public record struct AddressRequest
{
    public AddressRequest()
    {

    }

    public AddressRequest(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public string Street { get;set;}
    public string City{get;set;}
    public string State{get;set;}
    public string ZipCode { get; set; }
}


