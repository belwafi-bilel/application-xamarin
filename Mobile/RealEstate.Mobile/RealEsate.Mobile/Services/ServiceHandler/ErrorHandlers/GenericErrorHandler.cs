using System.Threading.Tasks;
using RealEstate.Mobile.Services.SAL;

namespace RealEstate.Mobile.Services.ServiceHandler.ErrorHandlers
{
    public class GenericErrorHandler : BaseErrorHandler
    {
        public override bool IsResponsible(RealEstateServicesResponseWrapper serviceResponse)
        {
			if(serviceResponse.Exception is TaskCanceledException) return false;
            return serviceResponse.IsSuccess == false;
        }

        protected override string FriendlyErrorTitle => "MOBILE_GENERIC_ERROR_TITLE".ToUpper();
        protected override string FriendlyErrorMessage => "MOBILE_GENERIC_ERROR_MESSAGE";
    }
}
