using CleanArchTemplate.Application.Common.Interfaces.Common;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Responses.Identity;
using CleanArchTemplate.Shared.Wrapper;

namespace CleanArchTemplate.Application.Common.Interfaces.Services.Identity
{
    public interface IRoleService : IService
    {
        Task<Result<List<RoleResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<Result<RoleResponse>> GetByIdAsync(string id);

        Task<Result<string>> SaveAsync(RoleRequest request);

        Task<Result<string>> DeleteAsync(string id);

        Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId);

        Task<Result<string>> UpdatePermissionsAsync(PermissionRequest request);
    }
}