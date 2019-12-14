using RealEstate.Mobile.ViewModels;
using RealEstate.Models.API.RealEstate;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealEstate.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductCategoryPage : ContentPage
    {
        ProductCategoryViewModel viewModel;
        public ProductCategoryPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ProductCategoryViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task.Run(async () =>
            {
                await viewModel.LoadData();
            });
        }

        private async void Handle_Refreshing(object sender, EventArgs e)
        {
            await viewModel.LoadData();
        }

        async void LocationList_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var categoryFilters = ProductFilters.Instance;
            categoryFilters.PageIndex = 1;
            categoryFilters.IsFromFilters = true;
            SetFiltersParams(categoryFilters);
            await Navigation.PushAsync(new ProductPage());
        }

        private void SetFiltersParams(ProductArguments productArguments)
        {
            //Clear filter before set new param
            productArguments.ProductCode = null;
            productArguments.Area = null;
            productArguments.Type = null;
            productArguments.PriceRange = null;
            productArguments.TownId = null;

            //Set new Category param
            productArguments.ListingTypeId = categoryList.SelectedItem != null ? (categoryList.SelectedItem as ListingTypeViewModel)?.Id : default(long?);
        }
    }
}