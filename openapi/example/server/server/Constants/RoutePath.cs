namespace server.Constants;

/// <summary>
/// Containing api path constants for API version 1. 
/// </summary>
public class RoutePath
{
    private const string BaseRoute = "api/v{version:apiVersion}";

    internal const string Main = BaseRoute + "/main";

    internal const string Test = BaseRoute + "/test";
}

