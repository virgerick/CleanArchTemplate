using System;
using Blazored.LocalStorage;
using CleanArchTemplate.Client.Services.LocalStorages;
using CleanArchTemplate.Shared.Constants.Storage;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Responses.Identity;

namespace CleanArchTemplate.Client.Storages;

public class MainStorage
{
    private readonly IEncrytedLocalStorageService _localStorageService;

    public MainStorage(IEncrytedLocalStorageService localStorageService)
	{
        _localStorageService = localStorageService;
        _localStorageService.Changed += UpdateState;
    }

    private void UpdateState(object? sender, ChangedEventArgs e)
    {
        if(e.Key == StorageConstants.Local.LoggedUserData)
        {
            UserLogged = (TokenResponse)e.NewValue ?? null!;
        }
    }

    public TokenResponse UserLogged { get; set; }



    public async ValueTask<TokenResponse> GetUserLoggedData(CancellationToken cancellationToken=default)
    {
       return await _localStorageService.GetItemAsync<TokenResponse>(StorageConstants.Local.LoggedUserData, cancellationToken);
    }
    public async ValueTask SetUserLoggedData(TokenResponse value,CancellationToken cancellationToken = default)
    {
        await _localStorageService.SetItemAsync<TokenResponse>(StorageConstants.Local.LoggedUserData, value, cancellationToken);
    }
} 

