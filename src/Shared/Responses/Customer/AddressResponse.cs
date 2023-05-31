using System.Reflection.Metadata;

namespace CleanArchTemplate.Shared.Responses.Customers;

public class AddressResponse
{
    public AddressResponse()
    {

    }       
    public AddressResponse(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public string Street{get;set;}
        public string City{get;set;}
        public string State{get;set;}
        public string ZipCode { get; set; }
}