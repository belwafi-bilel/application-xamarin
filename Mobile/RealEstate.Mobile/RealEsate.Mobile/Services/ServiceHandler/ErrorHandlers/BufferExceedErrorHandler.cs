using System.Net.Http;
using RealEstate.Mobile.Services.SAL;

namespace RealEstate.Mobile.Services.ServiceHandler.ErrorHandlers
{
    public class BufferExceedErrorHandler : BaseErrorHandler
    {
        public override bool IsResponsible(RealEstateServicesResponseWrapper serviceResponse)
        {
            if (serviceResponse.Exception is HttpRequestException httpExcetion)
            {
                return httpExcetion.Message?.Contains("Cannot write more bytes to the buffer") ?? false;
            }

            return false;
        }

        protected override string FriendlyErrorMessage =>
            "Buffer Exceeded Error";
    }
}
