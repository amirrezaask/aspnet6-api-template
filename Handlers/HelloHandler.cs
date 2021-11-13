namespace MinimalPlus.Handlers;

public class HelloHandler : IHttpHandler
{
    public static IResult HelloWorld() => Results.Ok();
    public WebApplication Map(string prefix, WebApplication app)
    {
        app.MapGet($"{prefix}/hello/world", HelloWorld);
        return app;
    }
}