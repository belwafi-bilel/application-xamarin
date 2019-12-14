using System.Net;
using RealEstate.Mobile.Services.SAL;

namespace RealEstate.Mobile.Services.ServiceHandler.ErrorHandlers
{
    public class UnauthorizedErrorHandler : BaseErrorHandler
    {
        public override bool IsResponsible(RealEstateServicesResponseWrapper serviceResponse)
        {
            return serviceResponse.HttpCode == HttpStatusCode.Unauthorized;
        }

        protected override string FriendlyErrorTitle => "MOBILE_UNAUTHORIZED_ERROR_TITLE";
        protected override string FriendlyErrorMessage => "MOBILE_UNAUTHORIZED_ERROR_MESSAGE";
    }
}
