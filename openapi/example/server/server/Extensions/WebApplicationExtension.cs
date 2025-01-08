namespace server.Extensions;

internal static class WebApplicationExtension
{
    internal static void UseSwaggerExtension(this WebApplication application)
    {
        application.UseSwagger();
        application.UseSwaggerUI(setupAction =>
        {
            var descriptions = application.DescribeApiVersions();
            foreach (var description in descriptions)
            {
                var urlSwaggerJSON = $"/swagger/{description.GroupName}/swagger.json";
                setupAction.SwaggerEndpoint(
                    urlSwaggerJSON,
                    description.GroupName.ToUpperInvariant());
            }
        });
    }
}
