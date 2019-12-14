using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using RealEstate.API.DAL;
using RealEstate.API.SAL.RealEstateService;
using RealEstate.Models.API.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.API.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public AuthorizationServerProvider()
        {
        }
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);         
            using (RealEstateEntities db = new RealEstateEntities())
            {
                var account = db.Accounts.Where(x => x.Email == context.UserName && x.IsDelete == false).Select(x => new LoginModel
                {
                    AccountId = x.AccountId,
                    Email = x.Email,
                    Password = x.Password,
                    FirstName = x.FirstName,
                    LastName = x.LastName
                }).FirstOrDefault();

                if(account == null)
                {
                    context.SetError("Invalid grant", "Tài khoản không tồn tại !");
                    return;
                }
                else
                {
                    string password = Common.Security.MD5Decrypt(account.Password);
                    if (context.Password != password)
                    {
                        context.SetError("Invalid grant", "Mật khẩu không đúng !");
                        return;
                    }
                    else
                    {
                        var roles = db.AccountRoles.Where(x => x.AccountId == account.AccountId && x.IsDelete == false)
                            .Select(x=>x.Role.RoleName).ToList();
                        if (roles != null)
                        {
                            var props = new AuthenticationProperties(new Dictionary<string, string>
                            {
                                { "roles", string.Join(",", roles) },
                                { "user_id", $"{account.AccountId}" },
                                { "name", $"{account.FirstName}" }

                            });

                            var ticket = new AuthenticationTicket(identity, props);
                            context.Validated(ticket);
                        }
                        else
                        {
                            context.SetError("Invalid grant", "Your account don't have role in system!");
                            return;
                        }
                    }
                }
            }  
        }
    }
}