using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RealEstate.Mobile.Services.SAL;
using RealEstate.Mobile.Services.ServiceHandler.ErrorHandlers;

namespace RealEstate.Mobile.Services.ServiceHandler
{
    public interface IServiceHandler
    {
        bool IsResponsible(RealEstateServicesResponseWrapper serviceResponse);
        Task Handle(RealEstateServicesResponseWrapper serviceResponse);
    }

    public class ServiceHandlerManager : List<IServiceHandler>
    {
        public async Task HandleErrors(RealEstateServicesResponseWrapper serviceResponse)
        {
            if (serviceResponse == null)
            {
                Debug.WriteLine("ServiceHandlerManager.cs -> HandleErrors() -> AulaServiceResponse was null");
            }
            else
            {
                IServiceHandler handler = this.FirstOrDefault(x => x.IsResponsible(serviceResponse));
                if (handler != null)
                {
                    await handler.Handle(serviceResponse);
                }
            }
        }
    }

    public static class ErrorHandlerManagerFactory
    {
        private static ServiceHandlerManager _fullServiceManager;

        public static ServiceHandlerManager FullServiceManager
        {
            get
            {
                if (_fullServiceManager != null) return _fullServiceManager;

                _fullServiceManager = new ServiceHandlerManager();
                _fullServiceManager.AddRange(new BaseErrorHandler[]
                {
                    new UnauthorizedErrorHandler(),
                    new BufferExceedErrorHandler(),
                    new NoNetworkErrorHandler(),
                    new GenericErrorHandler()
                });

                return _fullServiceManager;
            }
        }

        private static ServiceHandlerManager _noNetworkOnlyServiceHandlerManager;

        public static ServiceHandlerManager NoNetworkOnlyServiceHandlerManager
        {
            get
            {
                if (_noNetworkOnlyServiceHandlerManager != null) return _noNetworkOnlyServiceHandlerManager;

                _noNetworkOnlyServiceHandlerManager = new ServiceHandlerManager();
                _noNetworkOnlyServiceHandlerManager.AddRange(new BaseErrorHandler[]
                {
                    new NoNetworkErrorHandler(),
                });

                return _fullServiceManager;
            }
        }
    }
}
