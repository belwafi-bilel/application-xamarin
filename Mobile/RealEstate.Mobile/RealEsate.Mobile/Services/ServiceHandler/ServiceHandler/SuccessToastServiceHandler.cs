using System.Threading.Tasks;
using RealEstate.Mobile.Services.SAL;

namespace RealEstate.Mobile.Services.ServiceHandler.ServiceHandler
{
    public class SuccessToastServiceHandler : IServiceHandler
    {
        private string _message;

        public SuccessToastServiceHandler(string message = null)
        {
            _message = string.IsNullOrEmpty(message) ? "CONFIRMBOX_SUCCESS": message;
        }

        public bool IsResponsible(RealEstateServicesResponseWrapper serviceResponse)
        {
            return serviceResponse.IsSuccess;
        }

        public async Task Handle(RealEstateServicesResponseWrapper serviceResponse)
        {
        }
    }
}
