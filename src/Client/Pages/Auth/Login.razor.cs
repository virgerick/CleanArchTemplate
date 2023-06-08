using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using CleanArchTemplate.Client;
using CleanArchTemplate.Client.Extensions;
using CleanArchTemplate.Client.Shared;
using CleanArchTemplate.Client.Shared.Layouts;
using CleanArchTemplate.Client.Shared.Components;
using CleanArchTemplate.Client.Services;
using CleanArchTemplate.Shared.Enums;
using CleanArchTemplate.Shared.Wrapper;
using CleanArchTemplate.Shared.Models;
using CleanArchTemplate.Shared.Mapping;
using CleanArchTemplate.Shared.Requests;
using CleanArchTemplate.Shared.Responses;
using Radzen;
using Radzen.Blazor;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace CleanArchTemplate.Client.Pages.Auth;

public partial class Login
{
    
    TokenRequest Token=new();

    protected override async Task OnInitializedAsync()
    {
        var state = await _stateProvider.GetAuthenticationStateAsync();
        if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
        {
            _navigationManager.NavigateTo("/");
        }
    }

    async Task OnLogin(LoginArgs args)
    {
        await Excecutor.Run(async () =>
        {
            var request = new TokenRequest { Email = args.Username, Password = args.Password };
            var result = await AuthenticationManager.Login(request);
            result.ThrowIfNotSucceded();
            Console.WriteLine(result.ToJsonSerialize());


        });
        
    }


}
