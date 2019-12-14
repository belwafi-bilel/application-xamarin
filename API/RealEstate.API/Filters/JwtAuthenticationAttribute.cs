using Microsoft.IdentityModel.Tokens;
using RealEstate.API.DAL;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http.Filters;
using Claim = System.Security.Claims.Claim;
using ClaimTypes = System.Security.Claims.ClaimTypes;

namespace RealEstate.API.Filters
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => throw new NotImplementedException();

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
                return;

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateJwtToken(token);
            if (principal == null)
                return;
            else
                context.Principal = principal;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            string userId;
            if (GetUserIdByToken(token, out userId))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                    // Add more claims if needed: Roles, ...
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);
                return Task.FromResult(user);
            }
            return Task.FromResult<IPrincipal>(null);
        }
        bool GetUserIdByToken(string token, out string userId)
        {
            userId = null;

            var simplePrinciple = GetPrincipal(token);
            if (simplePrinciple == null)
                return false;

            var identity = simplePrinciple.Identity as ClaimsIdentity;
            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            userId = usernameClaim?.Value;
            if (string.IsNullOrEmpty(userId))
                return false;
            var accountId = Double.Parse(userId);
            using (RealEstateEntities db = new RealEstateEntities())
            {
                var userAccount = db.Accounts.FirstOrDefault(x => x.AccountId == accountId && x.IsDelete == true);
                return userAccount == null;
            }
        }
        ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(WebConfigurationManager.AppSettings["secretkey"]);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}