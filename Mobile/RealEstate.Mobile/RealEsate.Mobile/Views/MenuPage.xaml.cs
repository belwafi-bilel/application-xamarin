using RealEstate.Mobile.AppSettings;
using RealEstate.Mobile.Models;
using RealEstate.Mobile.Utils;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealEstate.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();
            bool isLoggedIn = JwtTokenHandler.Instance.IsLoggedIn().Result;
            string loginText = isLoggedIn ? "Logout" : "Login";
            string loginIcon = isLoggedIn ? "\uf2f6" : "\uf2f5";
            //if (App.AccountInfo != null)
            //{
            //    LabelName.Text = "Xin chào " + App.AccountInfo.Name;
            //}
            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Dashboard, Icon="\uf015" , Title="Dashboard" },
                new HomeMenuItem {Id = MenuItemType.ProductList, Icon="\uf0ca", Title="Estate Listing" },
                new HomeMenuItem {Id = MenuItemType.Location, Icon="\uf5a0", Title="Towns" },
                new HomeMenuItem {Id = MenuItemType.Category, Icon="\uf46d", Title="Estate Types"},
                new HomeMenuItem {Id = MenuItemType.MyProduct, Icon="\uf007", Title="My Listing" },
                new HomeMenuItem {Id = MenuItemType.ChangePassword, Icon="\uf502", Title="Change Password"},
                new HomeMenuItem {Id = MenuItemType.Login, Icon=loginIcon, Title=loginText},
                new HomeMenuItem {Id = MenuItemType.Help, Icon="\uf05a", Title="Help"},
            };

            ListViewMenu.ItemsSource = menuItems;
            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };            
    }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (App.AccountInfo != null)
            {
                LabelName.Text = "Welcome " + App.AccountInfo.Name;
            }
        }
    }
}