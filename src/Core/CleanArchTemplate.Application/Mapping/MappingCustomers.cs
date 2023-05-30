using System;
using CleanArchTemplate.Domain.Customers;
using CleanArchTemplate.Shared.Responses.Customers;

namespace CleanArchTemplate.Application.Mapping;

public static class MappingCustomers
{
	public static CustomerResponse Map(this Customer x)
	{
		return new CustomerResponse(x.Id.Value, x.Name, x.Email, x.Deleted, x.Address.Map());
	}
	public static AddressResponse Map(this Address x)
	{
		return new AddressResponse(x.Street, x.City, x.State, x.ZipCode);
	}
	
}

