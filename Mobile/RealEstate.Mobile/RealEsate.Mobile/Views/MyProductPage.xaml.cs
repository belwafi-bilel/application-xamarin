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
using RealEstate.Mobile.Utils;
using System.Threading;
using Rg.Plugins.Popup.Extensions;
using RealEstate.Mobile.AppSettings;

namespace RealEstate.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyProductPage : ContentPage
    {
        MyProductViewModel viewModel;
        private DialogService dialogService;
        static Semaphore semaphoreObject = new Semaphore(initialCount: 1, maximumCount: 1);

        public MyProductPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MyProductViewModel();
            viewModel.AllProductListView = productList;
            viewModel.OnExpiringListView = onExpiringProduct;
            viewModel.ExpiredListView = expiredProduct;
            viewModel.DeletedListView = deletedProduct;
            dialogService = new DialogService();
            MessagingCenter.Unsubscribe<string>(this, Settings.MyProductFilterKey);
            MessagingCenter.Subscribe<string>(this, Settings.MyProductFilterKey, (args) =>
            {
                LoadFilterData();
            });
            MessagingCenter.Unsubscribe<ProductDetailPage, ProductDetailViewModel>(this, Settings.MyProductReloadKey);
            MessagingCenter.Subscribe<ProductDetailPage, ProductDetailViewModel>(this, Settings.MyProductReloadKey, (sender, arg) =>
            {
                ReloadEditProduct(arg);
            });
        }
        async void Handle_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            ProductItemViewModel product = null;
            switch (ProductFilters.Instance.ProductType)
            {
                case ProductType.Deleted:
                    product = (ProductItemViewModel)deletedProduct.SelectedItem;
                    break;
                case ProductType.Expired:
                    product = (ProductItemViewModel)expiredProduct.SelectedItem;
                    break;
                case ProductType.WillBeExpired:
                    product = (ProductItemViewModel)onExpiringProduct.SelectedItem;
                    break;
                default:
                    product = (ProductItemViewModel)productList.SelectedItem;
                    break;
            }
            ProductFilters.Instance.IsLoadPreviousPosition = true;
            await Navigation.PushAsync(new ProductViewPage(product));
        }
        async void Handle_Refreshing(object sender, System.EventArgs e)
        {
            ProductFilters.Instance.PageIndex = 1;
            await viewModel.RefreshData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (!ProductFilters.Instance.IsFromFilters)
            {
                ProductFilters.Instance.ResetFilters();
            }
            if (!ProductFilters.Instance.IsLoadPreviousPosition)
            {
                Task.Run(async () =>
                {
                    await viewModel.LoadData();
                });
            }
            else
            {
                ProductFilters.Instance.IsLoadPreviousPosition = false;
            }
            productList.ResetSwipe();
            onExpiringProduct.ResetSwipe();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }

        private void LoadFilterData()
        {
            Task.Run(async () =>
            {
                await viewModel.LoadData();
            });
        }
        private void ReloadEditProduct(ProductDetailViewModel productDetailViewModel)
        {
            ProductItemViewModel product = null;
            switch (ProductFilters.Instance.ProductType)
            {
                case ProductType.WillBeExpired:
                    product = viewModel.WillBeExpiredProducts.FirstOrDefault(x => x.Id == productDetailViewModel.Id);
                    break;
                case ProductType.None:
                    product = viewModel.Products.FirstOrDefault(x => x.Id == productDetailViewModel.Id);
                    break;
                default:
                    break;
            }
            if (product != null)
            {
                product.Area = productDetailViewModel.Area;
                product.RentPrice = productDetailViewModel.RentPrice;
                product.SalePrice = productDetailViewModel.SalePrice;
                product.RentUnit = productDetailViewModel.SelectedRentUnit.Name;
                product.SaleUnit = productDetailViewModel.SelectedSaleUnit.Name;
                product.RentUnitId = productDetailViewModel.SelectedRentUnit.Id;
                product.SaleUnitId = productDetailViewModel.SelectedSaleUnit.Id;
                product.IsHot = productDetailViewModel.IsHot ?? false;
                product.OwnerInfo = new OwnerInfoViewModel
                {
                    Name = productDetailViewModel.OwnerName,
                    Mobile = productDetailViewModel.Mobile
                };
                product.UpdatedDate = DateTime.Now;
            }
        }
        private async void FloatingActionButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductDetailPage());
        }

        async void OnEdit_Tapped(object sender, System.EventArgs e)
        {
            ProductItemViewModel product = null;
            switch (ProductFilters.Instance.ProductType)
            {
                case ProductType.WillBeExpired:
                    product = viewModel.WillBeExpiredProducts.ElementAt(viewModel.ItemIndex);
                    break;
                default:
                    product = viewModel.Products.ElementAt(viewModel.ItemIndex);
                    break;
            }
            if (product != null)
            {
                ProductFilters.Instance.IsLoadPreviousPosition = true;
                ProductFilters.Instance.IsFromFilters = true;
                await Navigation.PushAsync(new ProductDetailPage(product, isFromProductPage: false));
            }
        }
        async void OnSort_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushPopupAsync(new SortingPopupPage(false));
        }

        async void OnFilter_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new FilterPage());
        }
        async void OnDelete_Tapped(object sender, System.EventArgs e)
        {
            var confirm = await dialogService.ShowConfirmedAsync("Bạn có muốn xoá sản phẩm này ?", "Xác nhận", "OK", "Cancel");
            if (confirm)
            {
                bool result = await viewModel.DeleteProduct();
                if (!result)
                {
                    await dialogService.ShowAlertAsync("Bạn không thể xoá sản phẩm. Vui lòng liên hệ admin", "Thông báo", "OK");
                }
                else
                    dialogService.ShowToast("Sản phẩm đã được xoá thành công");
            }
            productList?.ResetSwipe();
            onExpiringProduct?.ResetSwipe();
        }
        void Handle_SwipeStarted(object sender, Syncfusion.ListView.XForms.SwipeStartedEventArgs e)
        {
            viewModel.ItemIndex = -1;
        }

        void Handle_SwipeEnded(object sender, Syncfusion.ListView.XForms.SwipeEndedEventArgs e)
        {
            viewModel.ItemIndex = e.ItemIndex;
        }

        void OnTab_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            switch (e.Index)
            {
                case 0:
                    ProductFilters.Instance.ProductType = ProductType.None;
                    break;
                case 1:
                    ProductFilters.Instance.ProductType = ProductType.WillBeExpired;
                    break;
                case 2:
                    ProductFilters.Instance.ProductType = ProductType.Expired;
                    break;
                case 3:
                    ProductFilters.Instance.ProductType = ProductType.Deleted;
                    break;
                default:
                    break;
            }
            Console.WriteLine($"SELECTED TAB: {e.Index}");
            Task.Run(async () =>
            {
                await viewModel.LoadData();
            });
        }
    }
}