using CleanArchTemplate.Abstraction.Services;
using CleanArchTemplate.Application.Common.Configurations;
using CleanArchTemplate.Application.Common.Interfaces.Services;
using CleanArchTemplate.Application.Common.Interfaces.Services.Account;
using CleanArchTemplate.Application.Common.Interfaces.Services.Identity;
using CleanArchTemplate.Domain.Identity;
using CleanArchTemplate.Infrastructure;
using CleanArchTemplate.Infrastructure.Persistence.Database;
using CleanArchTemplate.Infrastructure.Services;
using CleanArchTemplate.Infrastructure.Services.Identity;
using CleanArchTemplate.Server.Localization;
using CleanArchTemplate.Shared.Constants.Application;
using CleanArchTemplate.Shared.Constants.Localization;
using CleanArchTemplate.Shared.Constants.Permission;
using CleanArchTemplate.Shared.Wrapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CleanArchTemplate.Server.Extensions;

public static class GlobalizationExtension
{
    public static IServiceCollection AddGlobalization(this IServiceCollection services)
    {
        const string defaultCulture = "en-US";
        var supportedCultures = LocalizationConstants.SupportedLanguages
            .Select(x=>new CultureInfo(x.Code))
            .ToList();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(defaultCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        services.AddLocalization(option => option.ResourcesPath = "Resources");
        return services;
    }

    public static IApplicationBuilder UseGlobalization(
    this WebApplication app)
    {
        app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
        return app;
    }
   
}
public static class ServiceCollectionExtensions
{
    internal static async Task<IStringLocalizer> GetRegisteredServerLocalizerAsync<T>(this IServiceCollection services) where T : class
    {
        var serviceProvider = services.BuildServiceProvider();
        var localizer = serviceProvider.GetService<IStringLocalizer<T>>();
        await serviceProvider.DisposeAsync();
        return localizer!;
    }
    internal static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddDbContext<ApplicationDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
        .AddTransient<IDatabaseSeeder, DatabaseSeeder>();

    internal static IServiceCollection AddCurrentUserService(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }

    internal static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }

    internal static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTimeService, SystemDateTimeService>();
        services.Configure<MailConfiguration>(configuration.GetSection("MailConfiguration"));
        services.AddTransient<IMailService, SMTPMailService>();
        return services;
    }

    internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IRoleClaimService, RoleClaimService>();
        services.AddTransient<ITokenService, IdentityService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IUserService, UserService>();
        services.AddScoped<IExcelService, ExcelService>();
        /* Todo:
        services.AddTransient<IChatService, ChatService>();
        services.AddTransient<IUploadService, UploadService>();
        services.AddTransient<IAuditService, AuditService>();
        */
        return services;
    }

    internal static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services, AppConfiguration config)
    {
        var key = Encoding.UTF8.GetBytes(config.Secret);
        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(async bearer =>
            {
                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero
                };

                var localizer = await GetRegisteredServerLocalizerAsync<ServerCommonResources>(services);

                bearer.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments(ApplicationConstants.SignalR.HubUrl)))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = c =>
                    {
                        if (c.Exception is SecurityTokenExpiredException)
                        {
                            c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            c.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(Result.Failure(localizer["The Token is expired."]));
                            return c.Response.WriteAsync(result);
                        }
                        else
                        {
#if DEBUG
                            c.NoResult();
                            c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
#else
                                c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                c.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject(Result.Failure(localizer["An unhandled error has occurred."]));
                                return c.Response.WriteAsync(result);
#endif
                        }
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        if (!context.Response.HasStarted)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(Result.Failure(localizer["You are not Authorized."]));
                            return context.Response.WriteAsync(result);
                        }

                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(Result.Failure(localizer["You are not authorized to access this resource."]));
                        return context.Response.WriteAsync(result);
                    },
                };
            });
        services.AddAuthorization(options =>
        {
            // Here I stored necessary permissions/roles in a constant
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    options.AddPolicy(propertyValue.ToString()!, policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString()!));
                }
            }
        });
        return services;
    }
}