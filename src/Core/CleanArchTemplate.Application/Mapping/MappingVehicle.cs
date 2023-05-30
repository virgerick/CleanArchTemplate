using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses.Vehicles;

namespace CleanArchTemplate.Application.Mapping;

public static class MappingVehicle
{
	public static VehicleResponse Map(this Vehicle x)
	{
		return new VehicleResponse(x.Id.Value, x.PlateNumber, x.Brand, x.Model, x.Type, x.Status, x.Deleted);
	}
	
	
}

