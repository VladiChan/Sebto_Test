using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SebTest.Data;
using SebTest.Interfaces;

namespace SebTest.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly TimeSpan Expiry = TimeSpan.FromMinutes(30);

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string BuildToken(User user)  
    {
        var key = _configuration["Jwt:Key"] 
                  ?? throw new InvalidOperationException("Jwt:Key is missing in configuration");

        var issuer = _configuration["Jwt:Issuer"] 
                     ?? throw new InvalidOperationException("Jwt:Issuer is missing");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,              
            claims: claims,
            expires: DateTime.UtcNow.Add(Expiry),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}