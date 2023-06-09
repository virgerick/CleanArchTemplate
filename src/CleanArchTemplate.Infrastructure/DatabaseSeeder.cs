﻿using System;
using CleanArchTemplate.Application.Common.Interfaces.Services;
using CleanArchTemplate.Domain.Identity;
using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Domain.Services;
using CleanArchTemplate.Infrastructure.Persistence.Database;
using CleanArchTemplate.Shared.Constants.Permission;
using CleanArchTemplate.Shared.Constants.Role;
using CleanArchTemplate.Shared.Constants.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CleanArchTemplate.Infrastructure;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly ILogger<DatabaseSeeder> _logger;
    private readonly IStringLocalizer<DatabaseSeeder> _localizer;
    private readonly ApplicationContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public DatabaseSeeder(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ApplicationContext context,
        ILogger<DatabaseSeeder> logger,
        IStringLocalizer<DatabaseSeeder> localizer)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _logger = logger;
        _localizer = localizer;
    }

    public void Initialize()
    {
        Migrate();
        AddAdministrator();
        AddDefaultService();
        AddDefaultRoute();
        _context.SaveChanges();
    }

    private void Migrate()
    { 
        _context.Database.Migrate();
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
            ApplicationUser.Create("superadmmin", "superadmin@yopmail.com", "Super","Administrator","",true,true)
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

    private void AddDefaultRoute()
    {
        Task.Run(async () =>
        {
            var repository = _context.Set<Route>();
            var empty= await repository.FirstOrDefaultAsync(x=>x.Id==RouteId.Empty);
            if (empty == null)
            {
                repository.Add(Route.Empty);
                await _context.SaveChangesAsync();
            }
        }).GetAwaiter().GetResult();;
    }

    private void AddDefaultService()
    {
        Task.Run(async () =>
        {
            var repository = _context.Set<Service>();
            var empty = await repository.FirstOrDefaultAsync(x => x.Id == ServiceId.Empty);
            if (empty == null)
            {
                repository.Add(Service.Empty);
                await _context.SaveChangesAsync();
            }
        }).GetAwaiter().GetResult();;
    }
}