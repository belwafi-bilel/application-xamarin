using System;
using Newtonsoft.Json;
using RealEstate.Mobile.Conf;
using RealEstate.Mobile.Models;
using RestSharp;

namespace RealEstate.Mobile.Services
{
    public class AuthenticationService
    {

        public LoginToken Login(string userName, string password)
        {
            var client = new RestClient(EnvironmentConf.Host);
            var request = new RestRequest("api/token", Method.POST);
            request.AddParameter("username", userName);
            request.AddParameter("password", password);
            request.AddParameter("grant_type", "password");
            IRestResponse response = client.Execute(request);
            LoginToken token = null;
            try
            {
                token = JsonConvert.DeserializeObject<LoginToken>(response.Content);
                if (string.IsNullOrEmpty(token.AccessToken))
                {
                    token.Message = response.Content;
                }
            }
            catch(Exception ex)
            {

            }
            return token;
        }
    }
}
