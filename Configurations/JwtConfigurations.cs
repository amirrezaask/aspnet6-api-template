namespace MinimalPlus.Configurations;


[Configuration("Jwt")]
public class JwtConfigurations
{
    public string Secret { get; set; }
    public TimeSpan ExpiresIn { get; set;  }
}