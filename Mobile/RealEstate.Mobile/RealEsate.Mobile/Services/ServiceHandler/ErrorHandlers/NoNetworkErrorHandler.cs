using System.Net;
using RealEstate.Mobile.Services.SAL;

namespace RealEstate.Mobile.Services.ServiceHandler.ErrorHandlers
{

    public class NoNetworkErrorHandler : BaseErrorHandler
    {
        public override bool IsResponsible(RealEstateServicesResponseWrapper serviceResponse)
        {
            return serviceResponse != null && serviceResponse.HttpCode == HttpStatusCode.InternalServerError &&
                   HttpClientManager.Instance.ConnectionUtils?.IsNetworkAvailable() == false;
        }

        protected override string OkString => "Ok";

        protected override string FriendlyErrorMessage =>
            "No Internet Connnection";

    }

}
