using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RealEstate.Mobile.Models;
using RealEstate.Mobile.Views;
using RealEstate.Mobile.ViewModels;
using RealEstate.Models.API.RealEstate;
using RealEstate.Mobile.Services;
using System.Reflection;
using Syncfusion.ListView.XForms;
using Rg.Plugins.Popup.Extensions;
using RealEstate.Mobile.AppSettings;

namespace RealEstate.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductPage : ContentPage
    {
        ProductViewModel viewModel;
        private DialogService dialogService;

        public ProductPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ProductViewModel();
            dialogService = new DialogService();
            viewModel.ProductListView = productList;
            MessagingCenter.Unsubscribe<string>(this, Settings.ProductFilterKey);
            MessagingCenter.Subscribe<string>(this, Settings.ProductFilterKey, (args) =>
            {
                LoadFilterData();
            });
            MessagingCenter.Unsubscribe<ProductDetailPage, ProductDetailViewModel>(this, Settings.ProductReloadKey);
            MessagingCenter.Subscribe<ProductDetailPage, ProductDetailViewModel>(this, Settings.ProductReloadKey, (sender, arg) =>
            {
                ReloadEditProduct(arg);
            });
        }

        async void Handle_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            ProductFilters.Instance.IsLoadPreviousPosition = true;
            var product = (ProductItemViewModel)productList.SelectedItem;
            await Navigation.PushAsync(new ProductViewPage(product));
        }
        async void Handle_Refreshing(object sender, System.EventArgs e)
        {
            ProductFilters.Instance.PageIndex = 1;
            await viewModel.RefreshData();
        }
        async void OnSort_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushPopupAsync(new SortingPopupPage(true));
        }

        async void OnFilter_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new FilterPage());
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(async () =>
            {
                ProductFilters.Instance.ProductType = ProductType.None;
                if (!ProductFilters.Instance.IsFromFilters)
                {
                    ProductFilters.Instance.ResetFilters();
                }
                if (!ProductFilters.Instance.IsLoadPreviousPosition)
                {
                    await viewModel.LoadData();
                }
                else
                {
                    ProductFilters.Instance.IsLoadPreviousPosition = false;
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    productList.ResetSwipe();

                });
            });
        }
        private void ReloadEditProduct(ProductDetailViewModel productDetailViewModel)
        {
            var product = viewModel.Products.FirstOrDefault(x => x.Id == productDetailViewModel.Id);
            if (product != null)
            {
                product.Area = productDetailViewModel.Area;
                product.RentPrice = productDetailViewModel.RentPrice;
                product.SalePrice = productDetailViewModel.SalePrice;
                product.RentUnit = productDetailViewModel.SelectedRentUnit?.Name;
                product.SaleUnit = productDetailViewModel.SelectedSaleUnit?.Name;
                product.RentUnitId = productDetailViewModel.SelectedRentUnit != null ? productDetailViewModel.SelectedRentUnit.Id : 0;
                product.SaleUnitId = productDetailViewModel.SelectedSaleUnit != null ? productDetailViewModel.SelectedSaleUnit.Id : 0;
                product.IsHot = productDetailViewModel.IsHot ?? false;
                product.OwnerInfo = new OwnerInfoViewModel
                {
                    Name = productDetailViewModel.OwnerName,
                    Mobile = productDetailViewModel.Mobile
                };
                product.UpdatedDate = DateTime.Now;
            }
        }
        private async void FilterItem_Clicked(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new FilterPage());
        }

        private async void FloatingActionButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductDetailPage());
        }

        async void OnEdit_Tapped(object sender, System.EventArgs e)
        {
            var product = viewModel.Products.ElementAt(viewModel.ItemIndex);
            if(product != null)
            {
                ProductFilters.Instance.IsLoadPreviousPosition = true;
                await Navigation.PushAsync(new ProductDetailPage(product));
            }
        }

        async void OnDelete_Tapped(object sender, System.EventArgs e)
        {
            var confirm = await dialogService.ShowConfirmedAsync("Are you sure to delete this product ?", "Confirmation", "OK", "Cancel");
            if(confirm)
            {
                bool result = await viewModel.DeleteProduct();
                if (!result)
                {
                    await dialogService.ShowAlertAsync("You're not allowed to delete this product. Please contact administrator", "Warning", "OK");
                }
                else
                    dialogService.ShowToast("Product deleted successfully!");
            }
            productList.ResetSwipe();
        }
        void Handle_SwipeStarted(object sender, Syncfusion.ListView.XForms.SwipeStartedEventArgs e)
        {
            viewModel.ItemIndex = -1;
        }
        private void LoadFilterData()
        {
            Task.Run(async () =>
            {
                await viewModel.LoadData();
            });
        }
        void Handle_SwipeEnded(object sender, Syncfusion.ListView.XForms.SwipeEndedEventArgs e)
        {
            viewModel.ItemIndex = e.ItemIndex;
        }
        void Handle_Swiping(object sender, Syncfusion.ListView.XForms.SwipingEventArgs e)
        {
            var product = viewModel.Products.ElementAt(e.ItemIndex);
            if (product != null && product.AllowDelete)
                e.Handled = false;
            else
                e.Handled = true;
        }

    }
}