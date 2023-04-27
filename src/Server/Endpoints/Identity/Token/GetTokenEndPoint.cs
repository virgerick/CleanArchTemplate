using System;
using CleanArchTemplate.Application.Common.Interfaces.Services.Identity;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Responses.Identity;
using CleanArchTemplate.Shared.Wrapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchTemplate.Server.Endpoints.Identity.Token;

public static class GetTokenEndPoint 
{
   public static WebApplication MapGetTokenEndPoint(this WebApplication app)
    {
        app.MapPost("/account/token", GetTokenAsync)
           .WithTags("Token")
           .WithName("GetToken")
           .WithDisplayName("GetToken");
        return app;
    }

    private static async Task<Result<TokenResponse>> GetTokenAsync(TokenRequest request, ITokenService tokenService)
    {
        var result = await tokenService.LoginAsync(request);
        return result;
    }
   
}

