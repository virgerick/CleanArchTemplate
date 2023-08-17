namespace CleanArchTemplate.Server.Endpoints.Services.Edit;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using CleanArchTemplate.Application.Services.Commands;
using CleanArchTemplate.Shared.Requests.Services;

public static class EditServiceEndPoint
{
     public static IEndpointConventionBuilder MapEditServiceEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPut("{Id}", EditServiceAsync)
        .WithName("EditService")
        .WithDisplayName("Edit Service");

        public static async ValueTask<Result<Guid>> EditServiceAsync(Guid id, AddEditServiceRequest request, ISender mediator,CancellationToken cancellationToken=default){
        var query = new EditServiceCommand(id, request.Name,request.Amount);
        var result = await mediator.Send(query, cancellationToken);
        return result.Match(
            response => Result.Success(response),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}
