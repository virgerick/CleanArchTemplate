using System;
using CleanArchTemplate.Shared.Requests.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchTemplate.Server.Endpoints.Identity.Token;

public static class GetTokenEndPoint 
{
   public static WebApplication MapGetTokenEndPoint(this WebApplication app)
   {
         app.MapGet("/account/token", ()=>
    {
            return Results.Ok("token requested");
        })
        .WithTags("Token")
        .WithName("GetToken")
        .WithDisplayName("GetToken"); 
        return app;
   }
}

