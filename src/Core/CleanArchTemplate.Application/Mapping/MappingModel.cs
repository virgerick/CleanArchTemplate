using CleanArchTemplate.Domain;
using CleanArchTemplate.Shared.Responses.Vehicles;

namespace CleanArchTemplate.Application.Mapping;

public static class MappingModel
{
	public static ModelResponse Map(this Model x)
	{
		return new ModelResponse(x.Id.Value, x.Name,x.Year,x.BrandId.Value,x.TypeId.Value);
	}
	
}

