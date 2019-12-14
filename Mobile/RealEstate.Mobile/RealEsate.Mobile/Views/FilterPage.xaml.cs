using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Mobile.ViewModels;
using RealEstate.Models.API.RealEstate;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace RealEstate.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FilterPage : ContentPage
	{
        FilterProductViewModel viewModel;

        public FilterPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new FilterProductViewModel();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            Task.Run(async () =>
            {
                viewModel.IsBusy = true;
                await viewModel.LoadData();
                FillForm();
                viewModel.IsBusy = false;
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
        }
        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            var productFilters = ProductFilters.Instance;
            productFilters.PageIndex = 1;
            ProductCodeEntry.Text = string.Empty;
            TownCombobox.SelectedItem = null;
            ListingTypeCombobox.SelectedItem = null;
            AreaCombobox.SelectedItem = null;
            TypeCombobox.SelectedItem = null;
            SalePriceCombobox.SelectedItem = null;
            RentPriceCombobox.SelectedItem = null;
            SetFiltersParams(productFilters);
            SearchtButton.Focus();
        }
        private void SetFiltersParams(ProductArguments productArguments)
        {
            productArguments.ProductCode = ProductCodeEntry.Text;
            productArguments.TownId = TownCombobox.SelectedItem != null ? (TownCombobox.SelectedItem as TownViewModel)?.Id : default(long?);
            productArguments.ListingTypeId = ListingTypeCombobox.SelectedItem != null ? (ListingTypeCombobox.SelectedItem as ListingTypeViewModel)?.Id : default(long?);
            productArguments.Area = AreaCombobox.SelectedItem != null ? (AreaCombobox.SelectedItem as AreaViewModel)?.AreaRange : null;
            productArguments.Type = TypeCombobox.SelectedItem != null ? (TypeCombobox.SelectedItem as TypeViewModel)?.Type : default(int?);
            if(productArguments.Type == 0)
            {
                productArguments.PriceRange = SalePriceCombobox.SelectedItem != null ? (SalePriceCombobox.SelectedItem as PriceViewModel)?.PriceRange : null;
            }
            else if (productArguments.Type == 1)
            {
                productArguments.PriceRange = RentPriceCombobox.SelectedItem != null ? (RentPriceCombobox.SelectedItem as PriceViewModel)?.PriceRange : null;
            }
        }
        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            var productFilters = ProductFilters.Instance;
            productFilters.PageIndex = 1;
            productFilters.IsFromFilters = true;
            SetFiltersParams(productFilters);
            await Navigation.PopAsync();
        }
        private void FillForm()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if(!string.IsNullOrEmpty(ProductFilters.Instance.ProductCode))
                {
                    ProductCodeEntry.Text = ProductFilters.Instance.ProductCode;
                }
                if (ProductFilters.Instance.TownId.HasValue)
                    TownCombobox.SelectedItem = viewModel.Towns.FirstOrDefault(x => x.Id == ProductFilters.Instance.TownId);
                if (ProductFilters.Instance.ListingTypeId.HasValue)
                    ListingTypeCombobox.SelectedItem = viewModel.ListingTypes.FirstOrDefault(x => x.Id == ProductFilters.Instance.ListingTypeId);
                if (ProductFilters.Instance.Area != null)
                    AreaCombobox.SelectedItem = viewModel.AreaRanges.FirstOrDefault(x => x.AreaRange.Equals(ProductFilters.Instance.Area));
                if (ProductFilters.Instance.Type.HasValue)
                    TypeCombobox.SelectedItem = viewModel.Types.FirstOrDefault(x => x.Type == ProductFilters.Instance.Type);
                if(ProductFilters.Instance.PriceRange != null)
                {
                    if (ProductFilters.Instance.Type == 0)
                    {
                        SalePriceCombobox.SelectedItem = viewModel.SalePriceRanges.FirstOrDefault(x => x.PriceRange.Equals(ProductFilters.Instance.PriceRange));
                        SalePriceLayout.IsVisible = true;
                        RentPriceLayout.IsVisible = false;
                    }
                    else
                    {
                        RentPriceCombobox.SelectedItem = viewModel.RentPriceRanges.FirstOrDefault(x => x.PriceRange.Equals(ProductFilters.Instance.PriceRange));
                        SalePriceLayout.IsVisible = false;
                        RentPriceLayout.IsVisible = true;
                    }
                }
            });

        }

        void OnTypes_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (e.Value == null || string.IsNullOrEmpty(e.Value.ToString()))
                return;
            var selectedItem = e.Value as TypeViewModel;
            switch(selectedItem.Type.Value)
            {
                case 1:
                    SalePriceLayout.IsVisible = false;
                    RentPriceLayout.IsVisible = true;
                    break;
                case 0:
                    SalePriceLayout.IsVisible = true;
                    RentPriceLayout.IsVisible = false;
                    break;
            }
        }
    }
}