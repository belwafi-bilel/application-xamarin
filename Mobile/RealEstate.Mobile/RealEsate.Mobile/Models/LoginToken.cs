using System;
using Newtonsoft.Json;

namespace RealEstate.Mobile.Models
{
    public class LoginToken
    {
        //[JsonProperty(PropertyName = "expires_in")]
       //public DateTime ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
        [JsonProperty(PropertyName = "message_response")]
        public MessageResponse MessageResponse { get; set; }
        public string Message { get; set; }
    }
    public class MessageResponse
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        public string Description { get; set; }
    }
}
