namespace CleanArchTemplate.Server.Endpoints.Drivers.Edit;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using CleanArchTemplate.Application.Drivers.Commands;
using CleanArchTemplate.Shared.Requests.Drivers;

public static class EditDriverEndPoint
{
     public static IEndpointConventionBuilder MapEditDriverEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPut("{Id}", EditDriverAsync)
        .WithName("EditDriver")
        .WithDisplayName("Edit Driver");

        public static async ValueTask<Result<Guid>> EditDriverAsync(Guid Id, AddEditDriverRequest request, ISender Mediator,CancellationToken cancellationToken=default){
        var query = new EditDriverCommand(Id, request.Name, request.PhoneNumber, request.License, request.HireDate);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => Result.Success(response),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}
