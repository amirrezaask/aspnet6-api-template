using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalPlus.Configurations;

namespace MinimalPlus;


public static class SetupExtensions
{
    public static WebApplicationBuilder WantSwagger(this WebApplicationBuilder builder)
    {
        // Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Description = "JWT Bearer Token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        builder.Services.AddEndpointsApiExplorer();
        return builder;
    }
    public static WebApplicationBuilder WantAuthentication(this WebApplicationBuilder builder)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetConfigurationOf<JwtConfigurations>().Secret))
        };
        // Adding token validation parameters
        builder.Services.AddSingleton(tokenValidationParameters);
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o => { o.TokenValidationParameters = tokenValidationParameters; });

        builder.Services.AddAuthorization();

        return builder;
    }
    public static WebApplicationBuilder WantDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddSqlite<ApplicationDatabaseContext>(builder.Configuration.GetConfigurationOf<DatabaseConfigurations>().ConnectionString);
        return builder;
    }
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SampleASPNETMinimalAPIs Cloud APIs");
                c.RoutePrefix = String.Empty;
            });
        }
        
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}