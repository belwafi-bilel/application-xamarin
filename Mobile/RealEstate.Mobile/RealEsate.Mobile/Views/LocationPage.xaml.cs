using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RealEstate.Mobile.Models;
using RealEstate.Mobile.ViewModels;
using RealEstate.Models.API.RealEstate;

namespace RealEstate.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationPage : ContentPage
    {
        LocationViewModel viewModel;
        public LocationPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new LocationViewModel();
        }
       
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task.Run(async () =>
            {
                await viewModel.LoadData();
            });
        }
        async void Handle_Refreshing(object sender, System.EventArgs e)
        {
            await viewModel.RefreshData();
        }

        async void LocationList_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var townFilters = ProductFilters.Instance;
            townFilters.PageIndex = 1;
            townFilters.IsFromFilters = true;
            SetFiltersParams(townFilters);
            await Navigation.PushAsync(new ProductPage());
        }

        private void SetFiltersParams(ProductArguments productArguments)
        {
            //Clear filter before set new param
            productArguments.ProductCode = null;
            productArguments.Area = null;
            productArguments.ListingTypeId = null;
            productArguments.Type = null;
            productArguments.PriceRange = null;

            //Set new TownId param 
            productArguments.TownId = locationList.SelectedItem != null ? (locationList.SelectedItem as TownViewModel)?.Id : default(long?);            
        }
        
    }
}