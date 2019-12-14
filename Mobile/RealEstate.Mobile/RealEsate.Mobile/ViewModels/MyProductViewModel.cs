using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using RealEstate.Mobile.Views;
using RealEstate.Models.API.RealEstate;
using RealEstate.Mobile.Services;
using RealEstate.Mobile.Extensions;
using System.Collections.Generic;
using Plugin.Messaging;
using Syncfusion.ListView.XForms;
using System.Threading;

namespace RealEstate.Mobile.ViewModels
{
    public class MyProductViewModel : BaseViewModel
    {
        private ProductService ProductService => new ProductService();
        public Command<object> LoadMoreProductsCommand { get; set; }

        public MyProductViewModel()
        {
            Title = " My Listing ";
            LoadMoreProductsCommand = new Command<object>(LoadMoreItems);
            Products.CollectionChanged += (sender, e) =>
            {
                HasProduct = Products != null && Products.Any();
            };
            WillBeExpiredProducts.CollectionChanged += (sender, e) =>
            {
                HasOnExpringProduct = WillBeExpiredProducts != null && WillBeExpiredProducts.Any();
            };
            ExpiredProducts.CollectionChanged += (sender, e) =>
            {
                HasExpiredProduct = ExpiredProducts != null && ExpiredProducts.Any();
            };
            DeletedProducts.CollectionChanged += (sender, e) =>
            {
                HasDeletedProduct = DeletedProducts != null && DeletedProducts.Any();
            };
        }
        private async Task<EstateResultDto> GetProducts(ProductArguments productArguments)
        {
            return await ProductService.GetMyProducts(productArguments);
        }

        private async Task<int> AddProducts(EstateResultDto productResults, SfListView targetListView, ObservableCollection<ProductItemViewModel> targetDataSource)
        {
            TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
            Device.BeginInvokeOnMainThread(() =>
            {
                if (productResults != null && productResults.EstateResults != null)
                {
                    targetListView.DataSource.BeginInit();
                    var products = productResults.EstateResults.Select(x => new ProductItemViewModel
                    {
                        Id = x.Id,
                        ProductName = x.EstateCode,
                        SalePrice = x.SalePrice ?? 0,
                        SaleUnitId = x.SaleUnit != null ? x.SaleUnit.SaleUnitId ?? 0 : 0,
                        SaleUnit = x.SaleUnit?.Name,
                        RentPrice = x.RentPrice ?? 0,
                        RentUnitId = x.RentUnit != null ? x.RentUnit.RentUnitId ?? 0 : 0,
                        RentUnit = x.RentUnit?.Name,
                        TownId = x.Town != null ? x.Town.TownId ?? 0 : 0,
                        TownName = x.Town?.Name,
                        ListingTypeId = x.Estate_TypeId ?? 0,
                        ListingName = x.NameEstate_Type,
                        HouseNumber = x.HouseNumber,
                        StaffInfo = new StaffInfoViewModel
                        {
                            Name = $"{x.Account?.FirstName} {x.Account?.LastName}",
                            Mobile = x.Account?.Mobile
                        },
                        OwnerInfo = new OwnerInfoViewModel
                        {
                            Name = x.OwnerName,
                            Mobile = x.Phone
                        },
                        Area = x.Area,
                        Notes = x.Note,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.ModifiedDate,
                        IsDeleted = x.IsDelete ?? false,
                        IsHot = x.IsHot ?? false,
                        Lat = x.Lat,
                        Long = x.Long,
                        MainPinText = x.MainPinText,
                        Address = x.Address,
                        ImageUrls = x.ImageUrls,
                        CreatorId = (int)(x.Account != null ? x.Account.AccountId ?? 0 : 0)
                    }).ToList();

                    targetDataSource.Clear();
                    foreach (var item in products)
                    {
                        targetDataSource.Add(item);
                    }
                    targetListView.DataSource.EndInit();
                    taskCompletionSource.SetResult(1);
                }
                else
                {
                    targetDataSource.Clear();
                    taskCompletionSource.SetResult(0);
                }
                UpdateCanLoadMore(productResults);
            });
            return await taskCompletionSource.Task;
        }
        private async Task<int> AddProducts(EstateResultDto productResults)
        {
            switch (ProductFilters.Instance.ProductType)
            {
                case ProductType.None:
                    return await AddProducts(productResults, AllProductListView, Products);
                case ProductType.WillBeExpired:
                    return await AddProducts(productResults, OnExpiringListView, WillBeExpiredProducts);
                case ProductType.Expired:
                    return await AddProducts(productResults, ExpiredListView, ExpiredProducts);
                case ProductType.Deleted:
                    return await AddProducts(productResults, DeletedListView, DeletedProducts);
                default:
                    return await AddProducts(productResults, AllProductListView, Products);
            }
        }

        private void UpdateCanLoadMore(EstateResultDto productResults)
        {
            switch (ProductFilters.Instance.ProductType)
            {
                case ProductType.None:
                    CanLoadMoreProduct = (productResults != null && productResults.EstateResults.Any()
                        && productResults.TotalRecord > ProductFilters.Instance.PageIndex * ProductFilters.Instance.PageLimit);
                    break;
                case ProductType.WillBeExpired:
                    CanLoadMoreOnExpringProduct = (productResults != null && productResults.EstateResults.Any()
                        && productResults.TotalRecord > ProductFilters.Instance.PageIndex * ProductFilters.Instance.PageLimit);
                    break;
                case ProductType.Expired:
                    CanLoadMoreExpiredProduct = (productResults != null && productResults.EstateResults.Any()
                        && productResults.TotalRecord > ProductFilters.Instance.PageIndex * ProductFilters.Instance.PageLimit);
                    break;
                case ProductType.Deleted:
                    CanLoadMoreDeletedProduct = (productResults != null && productResults.EstateResults.Any()
                        && productResults.TotalRecord > ProductFilters.Instance.PageIndex * ProductFilters.Instance.PageLimit);
                    break;
            }
        }

        private async void AddLoadMoreProducts(EstateResultDto productResults)
        {
            switch (ProductFilters.Instance.ProductType)
            {
                case ProductType.None:
                    await AddLoadMoreProducts(productResults, AllProductListView, Products);
                    break;
                case ProductType.WillBeExpired:
                    await AddLoadMoreProducts(productResults, OnExpiringListView, WillBeExpiredProducts);
                    break;
                case ProductType.Expired:
                    await AddLoadMoreProducts(productResults, ExpiredListView, ExpiredProducts);
                    break;
                case ProductType.Deleted:
                    await AddLoadMoreProducts(productResults, DeletedListView, DeletedProducts);
                    break;
            }
        }

        private async Task<int> AddLoadMoreProducts(EstateResultDto productResults, SfListView targetListView, ObservableCollection<ProductItemViewModel> targetDataSource)
        {
            TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
            Device.BeginInvokeOnMainThread(() =>
            {
                if (productResults != null && productResults.EstateResults != null)
                {
                    var products = productResults.EstateResults.Select(x => new ProductItemViewModel
                    {
                        Id = x.Id,
                        ProductName = x.EstateCode,
                        SalePrice = x.SalePrice ?? 0,
                        SaleUnitId = x.SaleUnit != null ? x.SaleUnit.SaleUnitId ?? 0 : 0,
                        SaleUnit = x.SaleUnit?.Name,
                        RentPrice = x.RentPrice ?? 0,
                        RentUnitId = x.RentUnit != null ? x.RentUnit.RentUnitId ?? 0 : 0,
                        RentUnit = x.RentUnit?.Name,
                        TownId = x.Town != null ? x.Town.TownId ?? 0 : 0,
                        TownName = x.Town?.Name,
                        ListingTypeId = x.Estate_TypeId ?? 0,
                        ListingName = x.NameEstate_Type,
                        HouseNumber = x.HouseNumber,
                        StaffInfo = new StaffInfoViewModel
                        {
                            Name = $"{x.Account?.FirstName} {x.Account?.LastName}",
                            Mobile = x.Account?.Mobile
                        },
                        OwnerInfo = new OwnerInfoViewModel
                        {
                            Name = x.OwnerName,
                            Mobile = x.Phone
                        },
                        Area = x.Area,
                        Notes = x.Note,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.ModifiedDate,
                        IsDeleted = x.IsDelete ?? false,
                        IsHot = x.IsHot ?? false,
                        Lat = x.Lat,
                        Long = x.Long,
                        MainPinText = x.MainPinText,
                        Address = x.Address,
                        ImageUrls = x.ImageUrls,
                        CreatorId = (int)(x.Account != null ? x.Account.AccountId ?? 0 : 0)
                    });
                    foreach (var item in products)
                    {
                        targetDataSource.Add(item);
                    }
                    var indexTo = targetDataSource.Count - ProductFilters.Instance.PageLimit;
                    targetListView.LayoutManager.ScrollToRowIndex(indexTo, Syncfusion.ListView.XForms.ScrollToPosition.End, true);
                    targetListView.ScrollTo(indexTo, Syncfusion.ListView.XForms.ScrollToPosition.End, true);
                    taskCompletionSource.SetResult(1);
                }
                else
                {
                    taskCompletionSource.SetResult(0);
                }
                UpdateCanLoadMore(productResults);
            });
            return await taskCompletionSource.Task;
        }

        public async Task LoadData()
        {
            try
            {
                IsBusy = true;
                await SendRequest();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task RefreshData()
        {
            try
            {
                IsRefreshing = true;
                await SendRequest();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task<int> SendRequest()
        {
            ProductFilters.Instance.AccountId = App.AccountInfo.Id;
            ProductFilters.Instance.PageIndex = 1;
            var products = await ProductService.GetMyProducts(ProductFilters.Instance);
            return await AddProducts(products);
        }

        public SfListView AllProductListView { get; set; }
        public SfListView OnExpiringListView { get; set; }
        public SfListView DeletedListView { get; set; }
        public SfListView ExpiredListView { get; set; }

        private ObservableCollection<ProductItemViewModel> _products = new ObservableCollection<ProductItemViewModel>();
        public ObservableCollection<ProductItemViewModel> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        private ObservableCollection<ProductItemViewModel> _expiredProducts = new ObservableCollection<ProductItemViewModel>();
        public ObservableCollection<ProductItemViewModel> ExpiredProducts
        {
            get { return _expiredProducts; }
            set { SetProperty(ref _expiredProducts, value); }
        }

        private ObservableCollection<ProductItemViewModel> _willBeExpiredProducts = new ObservableCollection<ProductItemViewModel>();
        public ObservableCollection<ProductItemViewModel> WillBeExpiredProducts
        {
            get { return _willBeExpiredProducts; }
            set { SetProperty(ref _willBeExpiredProducts, value); }
        }

        private ObservableCollection<ProductItemViewModel> _deletedProducts = new ObservableCollection<ProductItemViewModel>();
        public ObservableCollection<ProductItemViewModel> DeletedProducts
        {
            get { return _deletedProducts; }
            set { SetProperty(ref _deletedProducts, value); }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }
        internal int ItemIndex
        {
            get;
            set;
        }

        private bool _hasProduct = true;
        public bool HasProduct
        {
            get { return _hasProduct; }
            set { SetProperty(ref _hasProduct, value); }
        }
        private bool _canLoadMoreProduct = true;
        public bool CanLoadMoreProduct
        {
            get { return _canLoadMoreProduct; }
            set { SetProperty(ref _canLoadMoreProduct, value); }
        }

        private bool _hasOnExpringProduct = true;
        public bool HasOnExpringProduct
        {
            get { return _hasOnExpringProduct; }
            set { SetProperty(ref _hasOnExpringProduct, value); }
        }
        private bool _canLoadMoreOnExpringProduct = true;
        public bool CanLoadMoreOnExpringProduct
        {
            get { return _canLoadMoreOnExpringProduct; }
            set { SetProperty(ref _canLoadMoreOnExpringProduct, value); }
        }
        private bool _hasExpiredProduct = true;
        public bool HasExpiredProduct
        {
            get { return _hasExpiredProduct; }
            set { SetProperty(ref _hasExpiredProduct, value); }
        }
        private bool _canLoadMoreExpiredProduct = true;
        public bool CanLoadMoreExpiredProduct
        {
            get { return _canLoadMoreExpiredProduct; }
            set { SetProperty(ref _canLoadMoreExpiredProduct, value); }
        }
        private bool _hasDeletedProduct = true;
        public bool HasDeletedProduct
        {
            get { return _hasDeletedProduct; }
            set { SetProperty(ref _hasDeletedProduct, value); }
        }
        private bool _canLoadMoreDeletedProduct = true;
        public bool CanLoadMoreDeletedProduct
        {
            get { return _canLoadMoreDeletedProduct; }
            set { SetProperty(ref _canLoadMoreDeletedProduct, value); }
        }
        private async void LoadMoreItems(object obj)
        {
            IsBusy = true;
            ProductFilters.Instance.PageIndex += 1;
            var products = await GetProducts(ProductFilters.Instance);
            if (products == null || !products.EstateResults.Any())
            {
                ProductFilters.Instance.PageIndex -= 1;
            }
            else
            {
                AddLoadMoreProducts(products);
            }
            IsBusy = false;
        }

        public async Task<bool> DeleteProduct()
        {
            ProductItemViewModel product = null;
            switch (ProductFilters.Instance.ProductType)
            {
                case ProductType.WillBeExpired:
                    product = WillBeExpiredProducts.ElementAt(ItemIndex);
                    break;
                default:
                    product = Products.ElementAt(ItemIndex);
                    break;
            }
            if (product != null)
            {
                var arguments = new RealEstateDeleteEstateDto { Id = product.Id, AccountId = App.AccountInfo.Id, IsDelete = true };
                IsBusy = true;
                bool result = await ProductService.UpdateDeleteProduct(arguments);
                IsBusy = false;
                if (result)
                {
                    switch (ProductFilters.Instance.ProductType)
                    {
                        case ProductType.WillBeExpired:
                            WillBeExpiredProducts.RemoveAt(ItemIndex);
                            break;
                        default:
                            Products.RemoveAt(ItemIndex);
                            break;
                    }
                }
                ItemIndex = -1;
                return result;
            }
            return false;
        }
    }
}