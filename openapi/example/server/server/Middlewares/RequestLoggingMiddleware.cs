namespace server.Middlewares;

/// <summary>
/// A log middleware
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Constructs a RequestLoggingMiddleware 
    /// </summary>
    /// <param name="next"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    /// <summary>
    /// Invoke the middleware
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task Invoke(HttpContext context)
    {
        try
        {

            if (context?.Request?.Method == null || context?.Request?.Path == null)
            {
                Console.WriteLine("Request method or path is null.");
            }
            else
            {
                var method = context.Request.Method ?? "UNKNOWN_METHOD";
                var path = context.Request.Path.HasValue ? context.Request.Path.Value : "UNKNOWN_PATH";

                Console.WriteLine($"Incoming request: Method={method}, Path={path}");
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "HttpContext cannot be null.");
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in middleware: {ex.Message}");
            throw;
        }
    }
}

