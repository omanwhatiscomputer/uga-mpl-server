// public class UserHelpers
// {
//     public static string GetUserToken(User user, IConfiguration config)
//     {
//         var tokenRequest = new TokenGenerationRequest_DTO
//         {
//             Email = user.Email,
//             UserId = user.UserId,
//             CustomClaims = new Dictionary<string, JsonElement>{
//                 { "admin", JsonDocument.Parse(user.UserType==Role.Admin ? "true" : "false").RootElement }// see identity constants
//             }
//         };
//         TimeSpan tokenLifetime = TimeSpan.FromHours(Convert.ToDouble(config["JwtSettings:DefaultTokenLifetime"]));
//         string token = JwtHelpers.GenerateJwtToken(tokenRequest, config, tokenLifetime);
//         return token;
//     }



//     public static void AppendCookies(HttpResponse response, IConfiguration config, User user, string token)
//     {

//         double cookieLifetime = Convert.ToDouble(config["CookieLifetimeInHours"]);
//         var cookieOptions = new CookieOptions
//         {
//             HttpOnly = true,
//             Secure = true,
//             SameSite = SameSiteMode.None,
//             Expires = DateTime.UtcNow.AddHours(cookieLifetime)
//         };

//         // Add cookies with Partitioned attribute using raw header
//         var cookieHeader = $"jwt={token}; HttpOnly; Secure; SameSite=None; Partitioned; Path=/; Expires={cookieOptions.Expires.Value.ToUniversalTime():R}";
//         response.Headers.Append("Set-Cookie", cookieHeader);

//         var userIdCookieHeader = $"userId={user.UserId}; HttpOnly; Secure; SameSite=None; Partitioned; Path=/; Expires={cookieOptions.Expires.Value.ToUniversalTime():R}";
//         response.Headers.Append("Set-Cookie", userIdCookieHeader);
//     }
// }