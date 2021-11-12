namespace MinimalPlus.Configurations;

public class JwtConfigurations
{
    public string Secret { get; set; }
    public TimeSpan ExpiresIn { get; set;  }
}

public static class JwtConfigurationsExtension
{
    public static JwtConfigurations GetJwtConfigurations(this IConfiguration configuration) => 
        configuration.GetSection("Jwt").Get<JwtConfigurations>();
}