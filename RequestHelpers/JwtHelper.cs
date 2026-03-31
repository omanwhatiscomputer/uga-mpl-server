// public class JwtHelper
// {
//     public static string GenerateJwtToken(TokenGenerationRequest_DTO request, IConfiguration config, TimeSpan tokenLifetime)
//     {

//         var tokenHandler = new JwtSecurityTokenHandler();
//         var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"));

//         var claims = new List<Claim>
//         {
//             new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//             new (JwtRegisteredClaimNames.Sub, request.Email),
//             new (JwtRegisteredClaimNames.Email, request.Email),
//             new("userid", request.UserId.ToString())
//         };

//         foreach (var claimPair in request.CustomClaims)
//         {
//             var jsonElement = (JsonElement)claimPair.Value;
//             var valueType = jsonElement.ValueKind switch
//             {
//                 JsonValueKind.True => ClaimValueTypes.Boolean,
//                 JsonValueKind.False => ClaimValueTypes.Boolean,
//                 JsonValueKind.Number => ClaimValueTypes.Double,
//                 _ => ClaimValueTypes.String
//             };

//             var claim = new Claim(claimPair.Key, claimPair.Value.ToString(), valueType);
//             claims.Add(claim);
//         }

//         var tokenDescriptor = new SecurityTokenDescriptor
//         {
//             Subject = new ClaimsIdentity(claims),
//             Expires = DateTime.UtcNow.Add(tokenLifetime),
//             Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
//             IssuedAt = DateTime.UtcNow,
//             SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
//         };

//         var token = tokenHandler.CreateToken(tokenDescriptor);
//         var jwt = tokenHandler.WriteToken(token);
//         return jwt;
//     }
// }