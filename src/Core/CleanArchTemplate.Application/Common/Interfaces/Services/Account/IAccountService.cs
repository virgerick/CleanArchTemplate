

using CleanArchTemplate.Application.Common.Interfaces.Common;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Wrapper;

using System.Threading.Tasks;

namespace CleanArchTemplate.Application.Common.Interfaces.Services.Account
{
    public interface IAccountService : IService
    {
        Task<IResult> UpdateProfileAsync(UpdateProfileRequest model, string userId);

        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId);

        Task<IResult<string>> GetProfilePictureAsync(string userId);

        Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);
    }
}