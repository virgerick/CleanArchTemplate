﻿using CleanArchTemplate.Application.Common.Interfaces.Services;
using CleanArchTemplate.Application.Common.Interfaces.Services.Identity;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Responses.Identity;
using CleanArchTemplate.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using CleanArchTemplate.Shared.Constants.Permission;
using CleanArchTemplate.Shared.Constants.Role;
using Microsoft.EntityFrameworkCore;
using CleanArchTemplate.Domain.Identity;
using CleanArchTemplate.Infrastructure.Extensions;

namespace CleanArchTemplate.Infrastructure.Services.Identity;

public class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRoleClaimService _roleClaimService;
    private readonly IStringLocalizer<RoleService> _localizer;
    private readonly ICurrentUserService _currentUserService;
    public RoleService(
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager,
        IRoleClaimService roleClaimService,
        IStringLocalizer<RoleService> localizer,
        ICurrentUserService currentUserService)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _roleClaimService = roleClaimService;
        _localizer = localizer;
        _currentUserService = currentUserService;
    }

    public async Task<Result<string>> DeleteAsync(string id)
    {
        var existingRole = await _roleManager.FindByIdAsync(id);
        if (existingRole!.Name == RoleConstants.AdministratorRole || existingRole.Name == RoleConstants.BasicRole)
        {
            return  Result<string>.Success(string.Format(_localizer["Not allowed to delete {0} Role."], existingRole.Name));
        }
        bool roleIsNotUsed = true;
        var allUsers = await _userManager.Users.ToListAsync();
        foreach (var user in allUsers)
        {
            if (await _userManager.IsInRoleAsync(user, existingRole.Name!))
            {
                roleIsNotUsed = false;
            }
        }
        if (!roleIsNotUsed)
        {
            return  Result<string>.Success(string.Format(_localizer["Not allowed to delete {0} Role as it is being used."], existingRole.Name));
        }
        await _roleManager.DeleteAsync(existingRole);
        return  Result<string>.Success(string.Format(_localizer["Role {0} Deleted."], existingRole.Name));
        

    }

    System.Linq.Expressions.Expression<Func<ApplicationRole, RoleResponse>> MapResponse = r => new RoleResponse
    {
        Id = r.Id,
        Description = r.Description,
        Name = r.Name!

    };
    public async Task<Result<List<RoleResponse>>> GetAllAsync()
    {
        var roles = await _roleManager.Roles
            .Select(MapResponse)
            .ToListAsync();
        return  Result<List<RoleResponse>>.Success(roles);
    }

    public async Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId)
    {
        var model = new PermissionResponse();
        var allPermissions = GetAllPermissions();
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
        }
        else
        {
            model.RoleId = role.Id;
            model.RoleName = role.Name!;
            var roleClaimsResult = await _roleClaimService.GetAllByRoleIdAsync(role.Id);
            if (!roleClaimsResult.Succeeded)
            {
                model.RoleClaims = new List<RoleClaimResponse>();
                return await Result<PermissionResponse>.FailureAsync(roleClaimsResult.Messages);
            }
            else
            {
                var roleClaims = roleClaimsResult.Data;
                var allClaimValues = allPermissions.Select(a => a.Value).ToList();
                var roleClaimValues = roleClaims.Select(a => a.Value).ToList();
                var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
                foreach (var permission in allPermissions)
                {
                    if (authorizedClaims.Any(a => a == permission.Value))
                    {
                        permission.Selected = true;
                        var roleClaim = roleClaims.SingleOrDefault(a => a.Value == permission.Value);
                        if (roleClaim?.Description != null)
                        {
                            permission.Description = roleClaim.Description;
                        }
                        if (roleClaim?.Group != null)
                        {
                            permission.Group = roleClaim.Group;
                        }
                    }
                }
            }
        }
        model.RoleClaims = allPermissions;
        return  Result<PermissionResponse>.Success(model);
    }

    private List<RoleClaimResponse> GetAllPermissions()
    {
        var allPermissions = new List<RoleClaimResponse>();

        #region GetPermissions

        allPermissions.GetAllPermissions();

        #endregion GetPermissions

        return allPermissions;
    }

    public async Task<Result<RoleResponse>> GetByIdAsync(string id)
    {
        var roles = await _roleManager.Roles
            .Select(MapResponse)
            .SingleOrDefaultAsync(x => x.Id == id);

        return  Result<RoleResponse>.Success(roles!);
    }

    public async Task<Result<string>> SaveAsync(RoleRequest request)
    {
        ApplicationRole? existingRole=null;
        if (!string.IsNullOrEmpty(request.Id))
        {
             existingRole = await _roleManager.FindByIdAsync(request.Id);
            if (existingRole!.Name == RoleConstants.AdministratorRole || existingRole.Name == RoleConstants.BasicRole)
            {
                return await Result<string>.FailureAsync(string.Format(_localizer["Not allowed to modify {0} Role."], existingRole.Name));
            }
            existingRole.Name = request.Name;
            existingRole.NormalizedName = request.Name.ToUpper();
            existingRole.Description = request.Description;
            await _roleManager.UpdateAsync(existingRole);
            return  Result<string>.Success(string.Format(_localizer["Role {0} Updated."], existingRole.Name));
        }
         existingRole = await _roleManager.FindByNameAsync(request.Name);
        if (existingRole != null) return await Result<string>.FailureAsync(_localizer["Similar Role already exists."]);
        ApplicationRole role = null!;
        ApplicationRole.Create(request.Name, request.Description)
            .Switch(rol => role = rol, _ => { });
        var response = await _roleManager.CreateAsync(role!);
        if (!response.Succeeded)
        {
            return await Result<string>.FailureAsync(response.Errors.Select(e => _localizer[e.Description].ToString()).ToList());
        }
        return  Result<string>.Success(string.Format(_localizer["Role {0} Created."], request.Name));

        
    }

    public async Task<Result<string>> UpdatePermissionsAsync(PermissionRequest request)
    {
        try
        {
            var errors = new List<string>();
            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role!.Name == RoleConstants.AdministratorRole)
            {
                var currentUser = await _userManager.Users.SingleAsync(x => x.Id == _currentUserService.UserId);
                if (await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdministratorRole))
                {
                    return await Result<string>.FailureAsync(_localizer["Not allowed to modify Permissions for this Role."]);
                }
            }

            var selectedClaims = request.RoleClaims.Where(a => a.Selected).ToList();
            if (role.Name == RoleConstants.AdministratorRole)
            {
                if (!selectedClaims.Any(x => x.Value == Permissions.Roles.View)
                   || !selectedClaims.Any(x => x.Value == Permissions.RoleClaims.View)
                   || !selectedClaims.Any(x => x.Value == Permissions.RoleClaims.Edit))
                {
                    return await Result<string>.FailureAsync(string.Format(
                        _localizer["Not allowed to deselect {0} or {1} or {2} for this Role."],
                        Permissions.Roles.View, Permissions.RoleClaims.View, Permissions.RoleClaims.Edit));
                }
            }

            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            foreach (var claim in selectedClaims)
            {
                var addResult = await _roleManager.AddPermissionClaim(role, claim.Value);
                if (!addResult.Succeeded)
                {
                    errors.AddRange(addResult.Errors.Select(e => _localizer[e.Description].ToString()));
                }
            }

            var addedClaims = await _roleClaimService.GetAllByRoleIdAsync(role.Id);
            if (addedClaims.Succeeded)
            {
                foreach (var claim in selectedClaims)
                {
                    var addedClaim = addedClaims.Data.SingleOrDefault(x => x.Type == claim.Type && x.Value == claim.Value);
                    if (addedClaim != null)
                    {
                        claim.Id = addedClaim.Id;
                        claim.RoleId = addedClaim.RoleId;
                        var saveResult = await _roleClaimService.SaveAsync(claim);
                        if (!saveResult.Succeeded)
                        {
                            errors.AddRange(saveResult.Messages);
                        }
                    }
                }
            }
            else
            {
                errors.AddRange(addedClaims.Messages);
            }

            if (errors.Any())
            {
                return await Result<string>.FailureAsync(errors);
            }

            return  Result<string>.Success(_localizer["Permissions Updated."]);
        }
        catch (Exception ex)
        {
            return await Result<string>.FailureAsync(ex.Message);
        }
    }

    public async Task<int> GetCountAsync()
    {
        var count = await _roleManager.Roles.CountAsync();
        return count;
    }
}
