using System.Reflection;

namespace MinimalPlus.Configurations;
public class ConfigurationAttribute : Attribute
{
    public string ConfigurationKey { get; set; }
    public ConfigurationAttribute()
    {

    }
    public ConfigurationAttribute(string key)
    {
        ConfigurationKey = key;
    }
}

public static class Extensions
{
    public static WebApplicationBuilder WantConfigurations(this WebApplicationBuilder builder)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                var attribs = type.GetCustomAttributes(typeof(ConfigurationAttribute), false);
                if (attribs != null && attribs.Length > 0)
                {
                    var attr = (ConfigurationAttribute)attribs[0];
                    Console.WriteLine(attr.ConfigurationKey);
                    var obj = builder.Configuration.GetSection($"{attr.ConfigurationKey}").Get(type);
                    builder.Services.AddSingleton(type, obj);
                }
            }
        }
        return builder;
    }
    public static T GetConfigurationOf<T>(this IConfiguration configuration)
    {
        var attr = typeof(T).GetCustomAttribute<ConfigurationAttribute>();
        if (attr == null)
        {
            return default;
        }
        return (T)configuration.GetSection(attr.ConfigurationKey).Get(typeof(T));
    }

}