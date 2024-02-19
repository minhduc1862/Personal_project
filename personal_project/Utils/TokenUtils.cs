using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace personal_project.Utils
{
    public class TokenUtils
    {
        public static string Generate(string _email)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_email));
            var myIssuer = "http://localhost:5200/";
            var myAudience = "http://localhost:5200/Account/ResetPassword";

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, _email),
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = myIssuer,
                Audience = myAudience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
