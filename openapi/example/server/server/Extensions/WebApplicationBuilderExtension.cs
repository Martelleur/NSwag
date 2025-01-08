using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using server.Constants;
using System.Reflection;
using System.Security.AccessControl;

namespace server.Extensions;

internal static class WebApplicationBuilderExtension
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    internal static void AddOpenApiDocumentationExtension(this WebApplicationBuilder builder)
    {
        AddOpenApiVersioning(builder);
        AddOpenApiDocumentation(builder);
    }

    private static void AddOpenApiVersioning(WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(setupAction =>
        {
            setupAction.ReportApiVersions = true;
            setupAction.AssumeDefaultVersionWhenUnspecified = true;
            setupAction.DefaultApiVersion = new ApiVersion(
                Api.MAJOR_VERSION_ONE,
                Api.MINOR_VERSION_ONE);
        }).AddMvc().AddApiExplorer(setupAction =>
        {
            setupAction.SubstituteApiVersionInUrl = true;
        });
    }

    private static void AddOpenApiDocumentation(WebApplicationBuilder builder)
    {
        var apiVersionDescriptionProvider = builder.Services
            .BuildServiceProvider()
            .GetRequiredService<IApiVersionDescriptionProvider>();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(setupAction =>
        {
            foreach (
                var description in 
                apiVersionDescriptionProvider.ApiVersionDescriptions
            )
            {
                setupAction.SwaggerDoc(
                    $"{description.GroupName}",
                    new()
                    {
                        Title = "NSwager Test Server API",
                        Version = description.ApiVersion.ToString(),
                        Description = "An api used to test NSwager."
                    });
            }

            var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsFullPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

            setupAction.IncludeXmlComments(xmlCommentsFullPath);
        });
    }
}

