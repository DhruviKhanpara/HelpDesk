
using HelpDesk.TicketService.API.Extensions;
using HelpDesk.TicketService.Application;
using HelpDesk.TicketService.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

namespace HelpDesk.TicketService.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ---------- Serilog ----------
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Service", "TicketService"));

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto;
        });

        // ---------- Application services ----------
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGenConfiguration();

        var app = builder.Build();

        app.UseSerilogRequestLogging(); // logs one line per HTTP request, incl. status code + duration

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseGlobalExceptionHandler();

        app.UseForwardedHeaders();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
