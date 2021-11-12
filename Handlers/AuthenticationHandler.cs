using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalPlus.Configurations;
using MinimalPlus.Contracts.Requests;
using MinimalPlus.Models;

namespace MinimalPlus.Handlers;

public static class AuthenticationHandler
{
    public static WebApplication MapAuthenticationAPIs(this WebApplication app, string prefix)
    {
        // User APIs
        app.MapPost($"{prefix}/auth/register", AuthenticationHandler.Register);
        app.MapPost($"{prefix}/auth/login", AuthenticationHandler.Login);
        app.MapGet($"{prefix}/auth/validate", AuthenticationHandler.ValidateToken);
        return app;
    }
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
}