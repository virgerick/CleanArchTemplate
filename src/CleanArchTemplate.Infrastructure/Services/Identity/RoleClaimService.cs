using CleanArchTemplate.Application.Common.Interfaces.Services;
using CleanArchTemplate.Application.Common.Interfaces.Services.Identity;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Responses.Identity;
using CleanArchTemplate.Shared.Wrapper;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using CleanArchTemplate.Domain.Identity;
using CleanArchTemplate.Infrastructure.Persistence.Database;

namespace CleanArchTemplate.Infrastructure.Services.Identity;

public class RoleClaimService : IRoleClaimService
{
    private readonly IStringLocalizer<RoleClaimService> _localizer;
    private readonly ApplicationContext _db;

    public RoleClaimService(
        IStringLocalizer<RoleClaimService> localizer,
        ICurrentUserService currentUserService,
        ApplicationContext db)
    {
        _localizer = localizer;
        _db = db;
    }

    public async Task<Result<List<RoleClaimResponse>>> GetAllAsync()
    {
        var roleClaims = await _db.RoleClaims
            .Select(c => new RoleClaimResponse
            {
                Id = c.Id,
                Description = c.Description,
                Group = c.Group,
                RoleId = c.RoleId,
                Type = c.ClaimType!,
                Value = c.ClaimValue!
            }).ToListAsync();
        return await Result<List<RoleClaimResponse>>.SuccessAsync(roleClaims);
    }

    public async Task<int> GetCountAsync()
    {
        return await _db.RoleClaims.CountAsync();
       
    }

    public async Task<Result<RoleClaimResponse>> GetByIdAsync(int id)
    {
        var roleClaim = await _db.RoleClaims
            .Select(c=>new RoleClaimResponse
            {
                Id = c.Id,
                Description = c.Description,
                Group = c.Group,
                RoleId = c.RoleId,
                Type = c.ClaimType!,
                Value = c.ClaimValue!
            })
            .SingleOrDefaultAsync(x => x.Id == id);
        return await Result<RoleClaimResponse>.SuccessAsync(roleClaim!);
    }

    public async Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId)
    {
        var roleClaims = await _db.RoleClaims
            .Include(x => x.Role)
            .Where(x => x.RoleId == roleId)
             .Select(c => new RoleClaimResponse
             {
                 Id = c.Id,
                 Description = c.Description,
                 Group = c.Group,
                 RoleId = c.RoleId,
                 Type = c.ClaimType!,
                 Value = c.ClaimValue!
             })
            .ToListAsync();
        return await Result<List<RoleClaimResponse>>.SuccessAsync(roleClaims);
    }

    public async Task<Result<string>> SaveAsync(RoleClaimRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RoleId))
        {
            return await Result<string>.FailureAsync(_localizer["Role is required."]);
        }

        if (request.Id == 0)
        {
            var existingRoleClaim =
                await _db.RoleClaims
                    .SingleOrDefaultAsync(x =>
                        x.RoleId == request.RoleId && x.ClaimType == request.Type && x.ClaimValue == request.Value);
            if (existingRoleClaim != null)
            {
                return await Result<string>.FailureAsync(_localizer["Similar Role Claim already exists."]);
            }
            ApplicationRoleClaim roleClaim = null!;
            ApplicationRoleClaim.Create(request.Type, request.Value, request.RoleId, request.Group, request.Description)
                .Switch(claim => roleClaim = claim,
                _ => { });
            await _db.RoleClaims.AddAsync(roleClaim!);
            await _db.SaveChangesAsync();
            return await Result<string>.SuccessAsync(string.Format(_localizer["Role Claim {0} created."], request.Value));
        }
        else
        {
            var existingRoleClaim =
                await _db.RoleClaims
                    .Include(x => x.Role)
                    .SingleOrDefaultAsync(x => x.Id == request.Id);
            if (existingRoleClaim == null)
            {
                return await Result<string>.SuccessAsync(_localizer["Role Claim does not exist."]);
            }
            existingRoleClaim.Update(request.RoleId, request.Type, request.Value,request.Group,request.Description);
            _db.RoleClaims.Update(existingRoleClaim);
            await _db.SaveChangesAsync();
            return await Result<string>.SuccessAsync(string.Format(_localizer["Role Claim {0} for Role {1} updated."], request.Value, existingRoleClaim.Role!.Name));
            
        }
    }

    public async Task<Result<string>> DeleteAsync(int id)
    {
        var existingRoleClaim = await _db.RoleClaims
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (existingRoleClaim != null)
        {
            _db.RoleClaims.Remove(existingRoleClaim);
            await _db.SaveChangesAsync();
            return await Result<string>.SuccessAsync(string.Format(_localizer["Role Claim {0} for {1} Role deleted."], existingRoleClaim.ClaimValue, existingRoleClaim.Role!.Name));
        }
        else
        {
            return await Result<string>.FailureAsync(_localizer["Role Claim does not exist."]);
        }
    }
}
