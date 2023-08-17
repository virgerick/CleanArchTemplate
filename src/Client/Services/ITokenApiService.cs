using Refit;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Responses.Identity;
using CleanArchTemplate.Shared.Wrapper;

namespace CleanArchTemplate.Client;
public interface ITokenApiService
{private const string EndPoint = "/Api/token";
    [Post(EndPoint+"/GetToken")]
    public Task<Result<TokenResponse>> GetAsync(TokenRequest request,CancellationToken cancellationToken=default);
    [Post(EndPoint+"/RefreshToken")]
    public Task<Result<TokenResponse>> RefreshAsync(RefreshTokenRequest request,CancellationToken cancellationToken=default);
}
