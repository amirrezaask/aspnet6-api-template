namespace MinimalPlus.Configurations;
public class DatabaseConfigurations
{
    public string ConnectionString { get; set; }
}


public static class DatabaseConfigurationsExtension
{
    public static DatabaseConfigurations GetDatabaseConfigurations(this IConfiguration configuration)
    {
        return configuration.GetSection("Database").Get<DatabaseConfigurations>();
    }
}

