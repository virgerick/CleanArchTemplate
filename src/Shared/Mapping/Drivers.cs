using CleanArchTemplate.Shared.Requests.Drivers;
using CleanArchTemplate.Shared.Responses.Drivers;

namespace CleanArchTemplate.Shared.Mapping;

public static class Drivers
{
	public static AddEditDriverRequest Map(this DriverResponse x)
	{
		return new AddEditDriverRequest(x.Name, x.PhoneNumber,x.License,x.HireDate);
	}
}

