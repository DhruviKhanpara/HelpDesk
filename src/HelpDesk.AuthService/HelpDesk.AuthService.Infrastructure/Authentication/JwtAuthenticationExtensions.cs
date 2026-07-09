using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Infrastructure.Options;
using HelpDesk.AuthService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HelpDesk.AuthService.Infrastructure.Authentication;

public static class JwtAuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IRefreshTokenHasher, RefreshTokenHasher>();

        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtOptions = configuration
            .GetRequiredSection(JwtOptions.SectionName)
            .Get<JwtOptions>()
            ?? throw new InvalidOperationException("JWT configuration is missing.");

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,

                    IssuerSigningKey = signingKey,

                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // Token expired
                        // Invalid signature
                        // Invalid issuer

                        return Task.CompletedTask;
                    },

                    OnTokenValidated = context =>
                    {
                        // Optional:
                        // Check if user is active
                        // Check if account is locked

                        return Task.CompletedTask;
                    },

                    OnChallenge = async context =>
                    {
                        // Runs before returning 401 Unauthorized

                        context.HandleResponse();

                        var problem = new ProblemDetails
                        {
                            Title = "Unauthorized",
                            Detail = "Authentication is required to access this resource.",
                            Status = StatusCodes.Status401Unauthorized,
                            Instance = context.Request.Path,
                            Extensions =
                            {
                                ["traceId"] = context.HttpContext.TraceIdentifier
                            }
                        };

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/problem+json";

                        await context.Response.WriteAsJsonAsync(problem);
                    },

                    OnForbidden = async context =>
                    {
                        // Runs when authenticated but not authorized (403)

                        var problem = new ProblemDetails
                        {
                            Title = "Forbidden",
                            Detail = "You do not have permission to access this resource.",
                            Status = StatusCodes.Status403Forbidden,
                            Instance = context.Request.Path,
                            Extensions =
                            {
                                ["traceId"] = context.HttpContext.TraceIdentifier
                            }
                        };

                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/problem+json";

                        await context.Response.WriteAsJsonAsync(problem);
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }
}
