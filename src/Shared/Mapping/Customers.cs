using System;
using System.Runtime.CompilerServices;
using CleanArchTemplate.Shared.Requests.Customers;
using CleanArchTemplate.Shared.Responses.Customers;

namespace CleanArchTemplate.Shared.Mapping;

public static class Customers
{
	public static AddEditCustomerRequest Map(this CustomerResponse x)
	{
		return new AddEditCustomerRequest(x.Name, x.Email, x.Address.Map());
	}
	public static AddressRequest Map(this AddressResponse x)
	{
		return new AddressRequest(x.Street, x.City, x.State, x.ZipCode);
	}
}

