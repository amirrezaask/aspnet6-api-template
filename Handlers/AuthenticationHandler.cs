using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalPlus.Configurations;
using MinimalPlus.Models;

namespace MinimalPlus.Handlers;

public class AuthenticationHandler : IHandler
{
    public static async Task<string> Register(ApplicationDatabaseContext dbContext, JwtConfigurations config, User user)
    {
        user.Id = Guid.NewGuid().ToString();
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return createJWTToken(config, user);
    }

    public static async Task<IResult> Login(ApplicationDatabaseContext dbContext, JwtConfigurations configurations, LoginRequest req)
    {
        var user = await dbContext.Users.Where(u => u.Email == req.Email).FirstOrDefaultAsync();
        if (user == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(createJWTToken(configurations, user));
    }

    public static ClaimsPrincipal ValidateToken(TokenValidationParameters tokenValidationParameters, JwtConfigurations configurations, [FromQuery] string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var claims = handler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
        if (validatedToken != null)
        {
            return claims;
        }

        return null;
    }
    private static string createJWTToken(JwtConfigurations _config, User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));    
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);    
    
        var claims = new[] {    
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),    
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),    
            new Claim(JwtRegisteredClaimNames.Jti, user.Id)    
        };    
    
        var token = new JwtSecurityToken("aspnet6",    
            user.Email,    
            claims,
            expires: DateTime.Now.Add(_config.ExpiresIn),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public WebApplication Map(string prefix, WebApplication app)
    {
        app.MapPost($"{prefix}/auth/register", Register);
        app.MapPost($"{prefix}/auth/login", Login);
        app.MapGet($"{prefix}/auth/validate", ValidateToken);
        return app;
    }
}