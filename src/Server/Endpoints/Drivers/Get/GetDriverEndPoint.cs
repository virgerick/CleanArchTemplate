namespace CleanArchTemplate.Server.Endpoints.Drivers.GetDriver;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Application.Drivers.Queries;
using CleanArchTemplate.Shared.Responses.Drivers;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public static class GetDriverEndPoint
{
     public static IEndpointConventionBuilder MapGetDriverEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/", GetDriverAsync)
        .WithDisplayName("Get Driver");
        public static async Task<ResultList<DriverResponse>> GetDriverAsync(ISender Mediator,CancellationToken cancellationToken=default){
        var query = new GetDriverQuery();
        var result = await Mediator.Send(query, cancellationToken);
        return  result.Match(
            response =>response,
             error => ResultList<DriverResponse>.Failure(error.GetMessages().ToList())
        ) ;
    }
}