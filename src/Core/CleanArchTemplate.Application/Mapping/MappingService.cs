using CleanArchTemplate.Domain.Services;
using CleanArchTemplate.Shared.Responses.Services;

namespace CleanArchTemplate.Application.Mapping;

public static class MappingService
{
	public static ServiceResponse Map(this Service x)
	{
		return new ServiceResponse(x.Id.Value, x.Name, x.Amount,x.Status.Status,x.Deleted);
		
	}
	
	
}

