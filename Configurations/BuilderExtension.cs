namespace MinimalPlus.Configurations;

public static class BuilderExtension
{
    public static WebApplicationBuilder WantConfigurations(this WebApplicationBuilder builder)
    {
        var jwtConfigurations = builder.Configuration.GetJwtConfigurations();
        var databaseConfigurations = builder.Configuration.GetDatabaseConfigurations();

        builder.Services.AddSingleton(jwtConfigurations);
        builder.Services.AddSingleton(databaseConfigurations);
        
        return builder;
    }
}