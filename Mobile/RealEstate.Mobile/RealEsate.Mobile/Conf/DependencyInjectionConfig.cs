using System;
using System.Collections.Generic;
using System.Text;
using RealEstate.Mobile.Services;
using RealEstate.Mobile.Services.SAL;
using Xamarin.Forms.Internals;

namespace RealEstate.Mobile.Conf
{
    public static class DependencyInjectionConfig
    {
        public static void Initialize()
        {
            Xamarin.Forms.DependencyService.Register<SimpleService>();
            Xamarin.Forms.DependencyService.Register<ProductService>();
            Xamarin.Forms.DependencyService.Register<AuthenticationService>();
        }
    }
}
