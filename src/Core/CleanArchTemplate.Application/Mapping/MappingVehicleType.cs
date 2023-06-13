using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses;

namespace CleanArchTemplate.Application.Mapping;

public static class MappingVehicleType
{
	public static IdNameResponse Map(this VehicleType x)
	{
		return new IdNameResponse(x.Id.Value, x.Name);
	}
	
}

