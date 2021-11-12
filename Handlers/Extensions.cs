using System.Reflection;

namespace MinimalPlus.Handlers;

public static class Extensions
{
    public static WebApplication MapAPIs(this WebApplication app, string prefix)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(IHandler)))
                {
                    var h  = (IHandler) Activator.CreateInstance(type);
                    h.Map(prefix, app);
                }
            }
        }
        return app;
    }
}