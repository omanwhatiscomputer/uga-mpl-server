using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace uga_mpl_server.RequestHelpers;

public class TokenGenerationRequest
{
    public string Email { get; set; } = null!;
    public Guid UserId { get; set; }
    public Dictionary<string, object> CustomClaims { get; set; } = new();
}

public class JwtHelper
{
    public static string GenerateJwtToken(TokenGenerationRequest request, TimeSpan tokenLifetime)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, request.Email),
            new(JwtRegisteredClaimNames.Email, request.Email),
            new("userid", request.UserId.ToString())
        };

        foreach (var claimPair in request.CustomClaims)
        {
            var jsonElement = (JsonElement)claimPair.Value;
            var valueType = jsonElement.ValueKind switch
            {
                JsonValueKind.True => ClaimValueTypes.Boolean,
                JsonValueKind.False => ClaimValueTypes.Boolean,
                JsonValueKind.Number => ClaimValueTypes.Double,
                _ => ClaimValueTypes.String
            };
            claims.Add(new Claim(claimPair.Key, claimPair.Value.ToString()!, valueType));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(tokenLifetime),
            Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            IssuedAt = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
