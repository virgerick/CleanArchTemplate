using Refit;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Responses.Identity;
using CleanArchTemplate.Shared.Wrapper;

namespace CleanArchTemplate.Client;
public interface ITokenApiService
{
    [Post("/token/GetToken")]
    public Task<Result<TokenResponse>> GetAsync(TokenRequest request,CancellationToken cancellationToken=default);
    [Post("/token/RefreshToken")]
    public Task<Result<TokenResponse>> RefreshAsync(RefreshTokenRequest request,CancellationToken cancellationToken=default);
}
