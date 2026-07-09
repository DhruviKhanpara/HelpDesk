using HelpDesk.AuthService.API.Filters;
using Microsoft.OpenApi.Models;

namespace HelpDesk.AuthService.API.Extensions;

public static class SwaggerGenExtensions
{
    public static void AddSwaggerGenConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "HelpDesk Auth API",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter: Bearer {your JWT token}"
            });

            options.OperationFilter<AuthorizeCheckOperationFilter>();
        });
    }
}
