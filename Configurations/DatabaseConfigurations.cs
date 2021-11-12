namespace MinimalPlus.Configurations;


[Configuration("Database")]
public class DatabaseConfigurations
{
    public string? ConnectionString { get; set; }
}