using Autofac;
using RealEstate.API.DAL;
using RealEstate.API.SAL.RealEstateService;

namespace RealEstate.API
{
    internal static class AutoFacConfig
    {

        internal static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<RealEstateEntities>().As<RealEstateEntities>();

            builder.RegisterType<RealEstateService>().As<IRealEstateService>();
        }
    }
}