
using server.Extensions;
using server.Middlewares;

namespace server;

/// <summary>
/// Entry point of the program.
/// </summary>
public class Program
{
    /// <summary>
    /// Main method of program.
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var app = BuildWebApp(args);
        ConfigureHttpRequestPipeline(app);
    }


    private static WebApplication BuildWebApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.AddOpenApiDocumentationExtension();

        return builder.Build();
    }

    private static void ConfigureHttpRequestPipeline(WebApplication app)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerExtension();
        }
        
        if (app.Environment.IsProduction())
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthorization();

        app.MapControllers();
        
        app.Run();
    }
}