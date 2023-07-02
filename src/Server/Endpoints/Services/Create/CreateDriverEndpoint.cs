using CleanArchTemplate.Application.Services.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Requests.Services;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Services.Create;
public static class CreateServiceEndpoint
{
      public static IEndpointConventionBuilder MapCreateServiceEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPost("/", CreateAsync)
        .WithName("CreateService")
        .WithTags("Service")
        .WithDisplayName("Create a new Service");
        public static async ValueTask<Result<Guid>> CreateAsync(ISender Mediator, AddEditServiceRequest request,CancellationToken cancellationToken=default){
       
        var query = new CreateServiceCommand(request.Name,request.Amount);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match<Result<Guid>>(
            id => Result<Guid>.Success(id),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}