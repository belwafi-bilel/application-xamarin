using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;

namespace RealEstate.API.Providers
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        public CustomJwtFormat()
        {
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            var symmetricKey = Convert.FromBase64String(WebConfigurationManager.AppSettings["secretkey"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, data.Properties.Dictionary["user_id"]),
                    new Claim(ClaimTypes.Role, data.Properties.Dictionary["roles"]),
                     new Claim(ClaimTypes.Name, data.Properties.Dictionary["name"])
                }),
                Expires = now.AddDays(30),//load from config
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);
            return token;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}