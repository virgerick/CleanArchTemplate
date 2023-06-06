namespace CleanArchTemplate.Server.Endpoints.Services.GetService;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Application.Services.Queries;
using CleanArchTemplate.Shared.Responses.Services;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public static class GetServiceEndPoint
{
     public static IEndpointConventionBuilder MapGetServiceEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/", GetServiceAsync)
        .WithDisplayName("Get Service");
        public static async Task<ResultList<ServiceResponse>> GetServiceAsync(ISender Mediator,CancellationToken cancellationToken=default){
        var query = new GetServiceQuery();
        var result = await Mediator.Send(query, cancellationToken);
        return  result.Match(
            response =>response,
             error => ResultList<ServiceResponse>.Failure(error.GetMessages().ToList())
        ) ;
    }
}