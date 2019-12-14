using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using RealEstate.Mobile.AppSettings;
using RealEstate.Mobile.Utils;

namespace RealEstate.Mobile.Services.SAL
{
    public class HttpClientManager
    {
        public HttpClient HttpClient;
        public IConnectionUtils ConnectionUtils;
        static HttpClientManager _instance;

        HttpClientManager()
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            var token = JwtTokenHandler.Instance.GetToken().Result;
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            HttpClient.MaxResponseContentBufferSize = 512000;
            HttpClient.Timeout = TimeSpan.FromSeconds(30);

        }

        public static HttpClientManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    MakeNewInstance();
                }
                return _instance;
            }
        }

        public static void MakeNewInstance()
        {
            _instance = new HttpClientManager();
        }
    }

    public static class HttpManagerExtension
    {
        public static HttpClient FillAuthenticationHeader(this HttpClient httpClient)
        {
            if(httpClient.DefaultRequestHeaders.Authorization?.Parameter == null)
            {
                var token = JwtTokenHandler.Instance.GetToken().Result;
                httpClient.DefaultRequestHeaders.Authorization = null;

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
            return httpClient;
        }
    }
}
