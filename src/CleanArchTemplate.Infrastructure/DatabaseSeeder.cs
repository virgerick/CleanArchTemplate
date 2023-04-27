using System;
using CleanArchTemplate.Application.Common.Interfaces.Services;
using CleanArchTemplate.Domain.Identity;
using CleanArchTemplate.Infrastructure.Persistence.Database;
using CleanArchTemplate.Shared.Constants.Permission;
using CleanArchTemplate.Shared.Constants.Role;
using CleanArchTemplate.Shared.Constants.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CleanArchTemplate.Infrastructure;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly ILogger<DatabaseSeeder> _logger;
    private readonly IStringLocalizer<DatabaseSeeder> _localizer;
    private readonly ApplicationContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public DatabaseSeeder(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ApplicationContext db,
        ILogger<DatabaseSeeder> logger,
        IStringLocalizer<DatabaseSeeder> localizer)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _db = db;
        _logger = logger;
        _localizer = localizer;
    }

    public void Initialize()
    {
        AddAdministrator();
        _db.SaveChanges();
    }

    private void AddAdministrator()
    {
        Task.Run(async () =>
        {
            //Check if Role Exists
            ApplicationRole adminRole = default!;
            var adminRoleResult = ApplicationRole.Create(RoleConstants.AdministratorRole, _localizer["Administrator role with full permissions"]);
            adminRoleResult
            .Switch(
                role => adminRole = role,
                _ => { }
                );
            var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
            if (adminRoleInDb == null)
            {
                await _roleManager.CreateAsync(adminRole);
                adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                _logger.LogInformation(_localizer["Seeded Administrator Role."]);
            }
            //Check if User Exists
            ApplicationUser superUser = default!;
            ApplicationUser.Create("superadmmin", "superadmin@yopmail.com", "Super","Administrator")
            .Switch(user => superUser = user, _ => { });

            var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email!);
            if (superUserInDb == null)
            {
                await _userManager.CreateAsync(superUser, UserConstants.DefaultPassword);
                var result = await _userManager.AddToRoleAsync(superUser, RoleConstants.AdministratorRole);
                if (result.Succeeded)
                {
                    _logger.LogInformation(_localizer["Seeded Default SuperAdmin User."]);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError(error.Description);
                    }
                }
            }
            /*foreach (var permission in Permissions.GetRegisteredPermissions())
            {
                await _roleManager.AddPermissionClaim(adminRoleInDb, permission);
            }*/
        }).GetAwaiter().GetResult();
    }
}