using CleanArchTemplate.Domain;
using CleanArchTemplate.Shared.Responses.Vehicles;

namespace CleanArchTemplate.Application.Mapping;

public static class MappingBrand
{
	public static BrandResponse Map(this Brand x)
	{
		return new BrandResponse(x.Id.Value, x.Name,x.Logo);
	}
	
}

