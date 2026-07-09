using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Infrastructure.Options;
using HelpDesk.AuthService.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.AuthService.Infrastructure.Persistence;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddOptions<SeedDataOptions>()
            .Bind(configuration.GetSection(SeedDataOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContext<AuthDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("AuthDbConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName));

            options.AddInterceptors(sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());
        });

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<AuthDbContext>());

        return services;
    }
}
