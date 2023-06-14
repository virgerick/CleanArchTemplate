using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses.Vehicles;

namespace CleanArchTemplate.Application.Mapping;

public static class MappingVehicle
{
	public static VehicleResponse Map(this Vehicle x)
	{
		return new VehicleResponse(x.Id.Value, x.PlateNumber, x.Color, x.Status.Status, x.ModelId.Value, x.Model?.Name ?? "", x.Model?.Brand?.Name ?? "", x.Model?.Year ?? null, x.Deleted);
	}
	
	
}

