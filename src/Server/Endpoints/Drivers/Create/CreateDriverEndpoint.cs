using CleanArchTemplate.Application.Drivers.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Requests.Drivers;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Drivers.Create;
public static class CreateDriverEndpoint
{
      public static IEndpointConventionBuilder MapCreateDriverEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPost("/", CreateAsync)
        .WithName("CreateDriver")
        .WithTags("Driver")
        .WithDisplayName("Create a new Driver");
        public static async ValueTask<Result<Guid>> CreateAsync(ISender Mediator, AddEditDriverRequest request,CancellationToken cancellationToken=default){
       
        var query = new CreateDriverCommand(request.Name,request.PhoneNumber,request.License,request.HireDate);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match<Result<Guid>>(
            id => Result<Guid>.Success(id),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}