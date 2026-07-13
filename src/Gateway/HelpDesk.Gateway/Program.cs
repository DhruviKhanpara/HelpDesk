namespace HelpDesk.Gateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Load YARP configuration
        builder.Configuration.AddJsonFile("Configuration/reverseproxy.json", optional: false, reloadOnChange: true);

        builder.Services.AddHealthChecks();

        builder.Services
            .AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

        var app = builder.Build();

        app.MapHealthChecks("/health");

        app.MapReverseProxy();

        app.Run();
    }
}
