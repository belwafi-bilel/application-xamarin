using System.Threading.Tasks;
using RealEstate.Mobile.Conf;
using RealEstate.Mobile.Services.SAL;
using RealEstate.Mobile.Services.ServiceHandler;

namespace RealEstate.Mobile.Services
{
    public abstract class BaseService
    {
        protected SimpleService Service => Xamarin.Forms.DependencyService.Get<SimpleService>();
        protected abstract string Controller { get; }

        protected string GenerateUrl(string method = "")
        {
            return EnvironmentConf.ApiEndPoints + Controller + "/" + method;
        }

        public async Task HandleResponseShowError<TData, TError>(RealEstatetServicesResponseWrapper<TData, TError> response, params IServiceHandler[] handlers)
        {
            await HandleResponse(response, ErrorHandlerManagerFactory.FullServiceManager, handlers);
        }

        public async Task HandleResponseIgnoreError<TData, TError>(RealEstatetServicesResponseWrapper<TData, TError> response, params IServiceHandler[] handlers)
        {
            await HandleResponse(response, ErrorHandlerManagerFactory.NoNetworkOnlyServiceHandlerManager, handlers);
        }

        async Task HandleResponse<TData, TError>(RealEstatetServicesResponseWrapper<TData, TError> response, ServiceHandlerManager serviceManager, IServiceHandler[] handlers)
        {
            if (handlers != null)
            {
                serviceManager.InsertRange(0, handlers);
            }

            await serviceManager.HandleErrors(response);

            if (handlers != null)
            {
                serviceManager.RemoveRange(0, handlers.Length);
            }
        }
    }
}
