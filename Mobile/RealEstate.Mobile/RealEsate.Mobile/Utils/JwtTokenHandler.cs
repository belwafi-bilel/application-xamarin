using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using RealEstate.Mobile.Models;
using Xamarin.Essentials;

namespace RealEstate.Mobile.Utils
{
    public class JwtTokenHandler
    {
        private static JwtTokenHandler jwtTokenParser = new JwtTokenHandler();
        private const string TokenKey = "oauth_token";

        private JwtTokenHandler() { }

        public static JwtTokenHandler Instance
        {
            get
            {
                if (jwtTokenParser == null)
                    jwtTokenParser = new JwtTokenHandler();
                return jwtTokenParser;
            }
        }

        public async Task<bool> IsLoggedIn()
        {
            string token = await GetToken();
            if (string.IsNullOrEmpty(token))
                return false;
            var jwtToken = ParseToken(token);
            if(jwtToken == null || jwtToken.ValidTo.ToLocalTime() < DateTime.Now)
            {
                SecureStorage.RemoveAll();
                return false;
            }
            return true;
        }

        public async Task SaveToken(string token)
        {
            try
            {
                await SecureStorage.SetAsync(TokenKey, token);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }

        public async Task<string> GetToken()
        {
            try
            {
                return await SecureStorage.GetAsync(TokenKey);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public void RemoveToken()
        {
            SecureStorage.Remove(TokenKey);
        }
        public async Task SetAccountInfo()
        {
            string token = await GetToken();
            var jwtToken = ParseToken(token);
            if (jwtToken == null)
                return;

            var claims = jwtToken.Claims;
            var accountInfo = new AccountInfo();
            int accountId = Convert.ToInt32(claims.FirstOrDefault(x => x.Type == "nameid")?.Value);
            accountInfo.Id = accountId;
            accountInfo.Name = (claims.FirstOrDefault(x => x.Type == "unique_name")?.Value).ToString();
            string roles = claims.FirstOrDefault(x => x.Type == "role")?.Value;

            if (!string.IsNullOrEmpty(roles))
            {
                accountInfo.Roles = roles.Split(',').ToList();
            }
            App.AccountInfo = accountInfo;
        }

        private JwtSecurityToken ParseToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var readableToken = jwtHandler.CanReadToken(token);
            if (!readableToken)
                return null;
            var jwtToken = jwtHandler.ReadJwtToken(token);
            return jwtToken;
        }
    }
}
