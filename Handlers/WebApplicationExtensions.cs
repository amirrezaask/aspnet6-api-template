namespace MinimalPlus.Handlers;

public static class WebApplicationExtensions
{
    public static WebApplication MapAPIs(this WebApplication app)
    {
        var apiV1 = "/api/v1";
        
        // Map authentication APIs
        app.MapAuthenticationAPIs(apiV1);
        
        app.MapGet("/hello/world", HelloHandler.HelloWorld);
        
        return app;
    }
}