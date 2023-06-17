using System.Text.Json;
using Blazored.LocalStorage;
using CleanArchTemplate.Client.Services.Crypto;
using CleanArchTemplate.Shared.Extensions;

namespace CleanArchTemplate.Client.Services.LocalStorages;

public class EncrytedLocalStorageService :  IEncrytedLocalStorageService
{
    private readonly ILogger<EncrytedLocalStorageService> _logger;
    private readonly ICryptoService _cryptoService;
    private readonly ILocalStorageService _localStorageService;
    public EncrytedLocalStorageService(
        ILogger<EncrytedLocalStorageService> logger,
        ICryptoService cryptoService,
        ILocalStorageService localStorageService)
    {
        _logger=logger;
        _cryptoService = cryptoService;
        _localStorageService = localStorageService;
        Changed = (s, e) => { };
        Changing = (s, e) => { };

    }


    public event EventHandler<ChangingEventArgs> Changing;
    public event EventHandler<ChangedEventArgs> Changed;

    public async ValueTask ClearAsync(CancellationToken cancellationToken = default)
    {
        var keys =await  KeysAsync();
       await _localStorageService.ClearAsync(cancellationToken);
        foreach (var key in keys)
        {
            RaiseOnChanged(key, null!, null!);
        }
    }

    public async ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _localStorageService.ContainKeyAsync(key, cancellationToken);
    }

    

    public async ValueTask<string> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default)
    {
       
        return await GetItemInternalAsync<string>(key,cancellationToken);
    }

    public async ValueTask<T> GetItemAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        return await GetItemInternalAsync<T>(key,cancellationToken);
    }

    public async ValueTask<string> KeyAsync(int index, CancellationToken cancellationToken = default)
    {
        return await _localStorageService.KeyAsync(index, cancellationToken);
    }

    

    public ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default)
    {
        return  _localStorageService.KeysAsync(cancellationToken);
    }

    public ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
    {
        return _localStorageService.LengthAsync(cancellationToken);
    }
    public async ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
    {
        
        await _localStorageService.RemoveItemAsync(key);
        RaiseOnChanged(key, null!, null!);
    }

    public async ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        
        await _localStorageService.RemoveItemsAsync(keys,cancellationToken);
        foreach (var key in keys)
        {
            RaiseOnChanged(key, null!, null!);
        }
        
    }

    public async ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default)
    {
        await SetItemAsync<string>(key, data,cancellationToken);
    }
  
    public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException("key");
        }
        var json = data!.ToJsonSerialize();
        ChangingEventArgs e = await RaiseOnChangingAsync(key, data).ConfigureAwait(continueOnCapturedContext: false);
        if (!e.Cancel)
        {
            var encrytedData = await _cryptoService.EncryptAsync(json);
            await _localStorageService.SetItemAsync(key, encrytedData.Value, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            RaiseOnChanged(key, e.OldValue, data);
        }
    }
    private async Task<T> GetItemInternalAsync<T>(string key, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException("key");
        }
        var encryptedData = await _localStorageService.GetItemAsync<string>(key);
        return await _cryptoService.DecryptAsync<T>(encryptedData);
      
    }
    private void RaiseOnChanged(string key, object oldValue, object data)
    {
        ChangedEventArgs e = new ChangedEventArgs
        {
            Key = key,
            OldValue = oldValue,
            NewValue = data
        };
        this.Changed?.Invoke(this, e);
    }
    private async Task<ChangingEventArgs> RaiseOnChangingAsync(string key, object data)
    {
        ChangingEventArgs changingEventArgs = new ChangingEventArgs
        {
            Key = key
        };
        ChangingEventArgs changingEventArgs2 = changingEventArgs;
        changingEventArgs2.OldValue = await GetItemAsync<object>(key).ConfigureAwait(continueOnCapturedContext: false);
        changingEventArgs.NewValue = data;
        ChangingEventArgs changingEventArgs3 = changingEventArgs;
        this.Changing?.Invoke(this, changingEventArgs3);
        return changingEventArgs3;
    }
}
