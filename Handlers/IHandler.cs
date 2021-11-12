namespace MinimalPlus.Handlers;

public interface IHandler
{
    WebApplication Map(string prefix, WebApplication app);
}