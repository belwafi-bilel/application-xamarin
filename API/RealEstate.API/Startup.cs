using System;
using System.Configuration;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using RealEstate.API.App_Start;
using RealEstate.API.Providers;

[assembly: OwinStartup(typeof(RealEstate.API.Template.Startup))]
namespace RealEstate.API.Template
{
    /// <summary>
    /// Represents the entry point into an application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Specifies how the ASP.NET application will respond to individual HTTP request.
        /// </summary>
        /// <param name="app">Instance of <see cref="IAppBuilder"/>.</param>
        public void Configuration(IAppBuilder app)
        {
           
            //enable cors origin request
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            var myProvider = new AuthorizationServerProvider();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                Provider = myProvider,
                AccessTokenFormat = new CustomJwtFormat()
            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());            
            new ApiConfig(app)
               .ConfigureCorsMiddleware(ConfigurationManager.AppSettings["cors"])
               .ConfigureAufacMiddleware()
               .ConfigureFormatters()
               .ConfigureRoutes()
               .ConfigureExceptionHandling()
               .ConfigureSwagger()
               .UseWebApi();

        }
    }
}