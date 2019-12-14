using System;
using System.Collections.Generic;
using RealEstate.Mobile.AppSettings;
using RealEstate.Models.API.RealEstate;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;

namespace RealEstate.Mobile.Views
{
    public partial class SortingPopupPage : PopupPage
    {
        bool _isLoaded;
        private bool _isFilterOnProductPage;

        public SortingPopupPage(bool isFilterOnProductPage)
        {
            InitializeComponent();
            _isFilterOnProductPage = isFilterOnProductPage;
            BindData();
            _isLoaded = true;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void SendMessage()
        {
            if (_isLoaded)
            {
                if(_isFilterOnProductPage)
                    MessagingCenter.Send<string>("", Settings.ProductFilterKey);
                else
                    MessagingCenter.Send<string>("", Settings.MyProductFilterKey);
                Navigation.PopPopupAsync();
            } 
        }
        private void BindData()
        {
            switch(ProductFilters.Instance.SortingType)
            {
                case SortingType.Lasted:
                    lastedProducts.IsChecked = true;
                    break;
                case SortingType.SalePriceDescending:
                    salePriceDescending.IsChecked = true;
                    break;
                case SortingType.SalePriceAscending:
                    salePriceAscending.IsChecked = true;
                    break;
                case SortingType.RentPriceDescending:
                    rentPriceDescending.IsChecked = true;
                    break;
                case SortingType.RentPriceAscending:
                    rentPriceAscending.IsChecked = true;
                    break;
                case SortingType.AreaDescending:
                    areaDescending.IsChecked = true;
                    break;
                case SortingType.AreaAscending:
                    areaAscending.IsChecked = true;
                    break;
                case SortingType.ProductCodeAscending:
                    productCodeAscending.IsChecked = true;
                    break;
                case SortingType.ProductCodeDescending:
                    productCodeDescending.IsChecked = true;
                    break;
                case SortingType.Oldest:
                    oldestProducts.IsChecked = true;
                    break;
                default:
                    lastedProducts.IsChecked = true;
                    break;
            }
        }
        void OnLasted_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if(lastedProducts.IsChecked == true)
            {
                ProductFilters.Instance.SortingType = SortingType.Lasted;
                SendMessage();
            }
        }
        void OnSalePriceDescending_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (salePriceDescending.IsChecked == true)
            {
                ProductFilters.Instance.SortingType = SortingType.SalePriceDescending;
                SendMessage();
            }
        }
        void OnSalePriceAscending_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (salePriceAscending.IsChecked == true)
            {
                ProductFilters.Instance.SortingType = SortingType.SalePriceAscending;
                SendMessage();
            }
        }
        void OnRentPriceDescending_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (rentPriceDescending.IsChecked == true)
            {
                ProductFilters.Instance.SortingType = SortingType.RentPriceDescending;
                SendMessage();
            }
        }
        void OnRentPriceAscending_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (rentPriceAscending.IsChecked == true)
            {
                ProductFilters.Instance.SortingType = SortingType.RentPriceAscending;
                SendMessage();
            }
        }
        void OnAreaDescending_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (areaDescending.IsChecked == true)
            {
                ProductFilters.Instance.SortingType = SortingType.AreaDescending;
                SendMessage();
            }
        }
        void OnAreaAscending_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (areaAscending.IsChecked == true)
            {
                ProductFilters.Instance.SortingType = SortingType.AreaAscending;
                SendMessage();
            }
        }
        void OnOldest_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (oldestProducts.IsChecked == true)
            {
                ProductFilters.Instance.SortingType = SortingType.Oldest;
                SendMessage();
            }
        }
        void OnProductCodeAscending_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (productCodeAscending.IsChecked == true)
            {
                ProductFilters.Instance.SortingType = SortingType.ProductCodeAscending;
                SendMessage();
            }
        }
        void OnProductCodeDescending_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (productCodeDescending.IsChecked == true)
            {
                ProductFilters.Instance.SortingType = SortingType.ProductCodeDescending;
                SendMessage();
            }
        }
    }
}
