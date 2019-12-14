using System.Threading.Tasks;
using RealEstate.Mobile.Services.SAL;

namespace RealEstate.Mobile.Services.ServiceHandler.ErrorHandlers
{
    public abstract class BaseErrorHandler : IServiceHandler
    {
        public abstract bool IsResponsible(RealEstateServicesResponseWrapper serviceResponse);

        protected virtual string FriendlyErrorTitle { get; } = "ATTENTION";

        protected abstract string FriendlyErrorMessage { get; }

        protected virtual string OkString => "CONFIRMBOX_OK_BTN";

		protected virtual bool IsIgnoreEnvironment => false;

		protected virtual string GetMessage(RealEstateServicesResponseWrapper serviceResponse = null)
        {
            return FriendlyErrorMessage;
        }

        protected virtual string GetTitle(RealEstateServicesResponseWrapper serviceResponse = null)
        {

            return FriendlyErrorTitle;
        }

        public async Task Handle(RealEstateServicesResponseWrapper serviceResponse)
        {
            await ShowDialog(serviceResponse);
        }

        public async Task ShowDialog(RealEstateServicesResponseWrapper serviceResponse = null)
        {
            string message = GetMessage(serviceResponse);
            string title = GetTitle(serviceResponse);
            string okBtn = OkString;
        }
    }
}
