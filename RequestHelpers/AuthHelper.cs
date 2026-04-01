using uga_mpl_server.Entities;

namespace uga_mpl_server.RequestHelpers;

public class AuthHelper
{
    public static string GetUserToken(User user, IConfiguration config)
    {
        var tokenRequest = new TokenGenerationRequest
        {
            Email = user.Email,
            UserId = user.Id,
            CustomClaims = new Dictionary<string, object>()
        };
        var tokenLifetime = TimeSpan.FromHours(Convert.ToDouble(config["JwtTokenLifetimeInHours"]));
        return JwtHelper.GenerateJwtToken(tokenRequest, tokenLifetime);
    }
}
