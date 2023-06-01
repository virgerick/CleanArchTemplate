using CleanArchTemplate.Domain.Drivers;
using CleanArchTemplate.Shared.Responses.Drivers;

namespace CleanArchTemplate.Application.Mapping;

public static class MappingDriver
{
	public static DriverResponse Map(this Driver x)
	{
		return new DriverResponse(x.Id.Value,x.Name,x.PhoneNumber,x.License,x.HireDate);
	}
	
	
}

