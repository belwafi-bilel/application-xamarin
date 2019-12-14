using RealEstate.Mobile.AppSettings;
using RealEstate.Mobile.Models;
using RealEstate.Mobile.Utils;
using RealEstate.Models.API.RealEstate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealEstate.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        private NavigationPage NavigationDetailPage => (NavigationPage)Detail;

        public MainPage()
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                bool isLoggedIn = await JwtTokenHandler.Instance.IsLoggedIn();
                if(!isLoggedIn)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.PushModalAsync(new NavigationPage(new LoginPage()), true);
                    });
                }
                else
                {
                    await JwtTokenHandler.Instance.SetAccountInfo();
                }
            });
            MasterBehavior = MasterBehavior.Popover;
            Detail = new NavigationPage(new DashboardPage());
        }

        public async Task NavigateFromMenu(int id)
        {
            switch (id)
            {
                case (int)MenuItemType.Dashboard:
                    Detail = new NavigationPage(new DashboardPage());
                    break;
                case (int)MenuItemType.ProductList:
                    ProductFilters.Instance.IsFromFilters = false;
                    Detail = new NavigationPage(new ProductPage());
                    break;
                case (int)MenuItemType.MyProduct:
                    ProductFilters.Instance.IsFromFilters = false;
                    Detail = new NavigationPage(new MyProductPage());
                    break;
                case (int)MenuItemType.Location:
                    Detail = new NavigationPage(new LocationPage());
                    break;
                case (int)MenuItemType.Category:
                    Detail = new NavigationPage(new ProductCategoryPage());
                    break;
                case (int)MenuItemType.ChangePassword:
                    Detail = new NavigationPage(new ChangePassword());
                    break;
                case (int)MenuItemType.Login:
                    string token = await JwtTokenHandler.Instance.GetToken();
                    if (!string.IsNullOrEmpty(token))
                    {
                        JwtTokenHandler.Instance.RemoveToken();
                        await Navigation.PushModalAsync(new LoginPage());
                    }
                    break;
                case (int)MenuItemType.Help:
                    Detail = new NavigationPage(new HelpPage());
                    break;
            }
            IsPresented = false;
        }
    }
}