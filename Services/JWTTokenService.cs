using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DogRallyMVC.Services
{
    public class JWTTokenService : IJWTTokenService
    {
            public bool IsUserAdmin(string token)
            {
                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var roleClaims = jwtToken.Claims.Where(claim => claim.Type == ClaimTypes.Role && claim.Value == "Admin");
                return roleClaims.Any();
            }
    }
}
