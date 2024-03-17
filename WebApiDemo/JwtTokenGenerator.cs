using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApiDemo
{
    public class JwtTokenGenerator
    {
        public string GenerateToken()
        {
            // symmetric, asymmetric encription key
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes("Carrotcarrotcarrot0."));
            // identity information about encription (security key, encription algorithm)
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

            // claim - given rights
            List<Claim> claims = new();
            claims.Add(new Claim(ClaimTypes.Role, "Member"));

            // JWT Token. JWT stands for JSON Web Token, which is a standard for representing claims securely between two parties.
            // It is mostly used for authentication and authorization purposes.
            // create a Json Web Token
            JwtSecurityToken token = new(issuer:"http://localhost", claims:claims, audience:"http://localhost",
                                        notBefore:DateTime.Now, expires:DateTime.Now.AddMinutes(2), signingCredentials:credentials);
            JwtSecurityTokenHandler handler = new();

            return handler.WriteToken(token);
        }
    }
}
