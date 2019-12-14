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

namespace RealEstate.Mobile.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private ProductService ProductService => new ProductService();
        public Command<object> LoadMoreProductsCommand { get; set; }

        public ProductViewModel()
        {
            Title = " Estates Listing ";
            LoadMoreProductsCommand = new Command<object>(LoadMoreItems);
            Products.CollectionChanged += (sender, e) =>
            {
                HasProduct = Products != null && Products.Any();
            };
        }
        private async Task<EstateResultDto> GetProducts(ProductArguments productArguments)
        {
            return await ProductService.GetProducts(productArguments);
        }
        private async Task<int> AddProducts(EstateResultDto productResults)
        {
            TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
            Device.BeginInvokeOnMainThread(() =>
            {
                if (productResults != null && productResults.EstateResults != null)
                {
                    ProductListView.DataSource.BeginInit();
                    var items = productResults.EstateResults.Select(x => new ProductItemViewModel
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
                    Products.Clear();
                    foreach (var item in items)
                    {
                        Products.Add(item);
                    }
                    ProductListView.DataSource.EndInit();
                    taskCompletionSource.SetResult(1);
                }
                else
                {
                    Products.Clear();
                    taskCompletionSource.SetResult(0);
                };
                CanLoadMore = CheckCanLoadMore(productResults);
            });
            return await taskCompletionSource.Task;
        }

        private bool CheckCanLoadMore(EstateResultDto productResults)
        {
            return (productResults != null && productResults.EstateResults.Any()
                && productResults.TotalRecord > ProductFilters.Instance.PageIndex * ProductFilters.Instance.PageLimit);
        }

        private bool _hasProduct = true;
        public bool HasProduct
        {
            get { return _hasProduct; }
            set { SetProperty(ref _hasProduct, value); }
        }
        private async Task<int> AddLoadMoreProducts(EstateResultDto productResults)
        {
            TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (productResults != null && productResults.EstateResults != null)
                    {
                        var items = productResults.EstateResults.Select(x => new ProductItemViewModel
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
                        foreach (var item in items)
                        {
                            Products.Add(item);
                        }
                        var indexTo = Products.Count - ProductFilters.Instance.PageLimit;
                        ProductListView.LayoutManager.ScrollToRowIndex(indexTo, Syncfusion.ListView.XForms.ScrollToPosition.End, true);
                        ProductListView.ScrollTo(indexTo, Syncfusion.ListView.XForms.ScrollToPosition.End, true);
                        taskCompletionSource.SetResult(1);
                    }
                    CanLoadMore = CheckCanLoadMore(productResults);
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetResult(0);
                }
            });
            return await taskCompletionSource.Task;
        }

        public async Task LoadData()
        {
            try
            {
                IsBusy = true;
                ProductFilters.Instance.AccountId = default(int?);
                ProductFilters.Instance.PageIndex = 1;
                var products = await GetProducts(ProductFilters.Instance);
                await AddProducts(products);
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
                ProductFilters.Instance.AccountId = default(int?);
                ProductFilters.Instance.PageIndex = 1;
                var products = await GetProducts(ProductFilters.Instance);
                await AddProducts(products);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        internal SfListView ProductListView { get; set; }
        internal ExtendedScrollView ExtendedScrollView { get; set; }
        private ObservableCollection<ProductItemViewModel> _products = new ObservableCollection<ProductItemViewModel>();
        public ObservableCollection<ProductItemViewModel> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }
        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }
        private bool _canLoadMore = false;
        public bool CanLoadMore
        {
            get { return _canLoadMore; }
            set { SetProperty(ref _canLoadMore, value); }
        }
        internal int ItemIndex
        {
            get;
            set;
        }
        internal int LastIndex { get; set; }
        private async void LoadMoreItems(object obj)
        {
            try
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
                    await AddLoadMoreProducts(products);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<bool> DeleteProduct()
        {
            var product = Products.ElementAt(ItemIndex);
            if (product != null)
            {
                var arguments = new RealEstateDeleteEstateDto { Id = product.Id, AccountId = App.AccountInfo.Id, IsDelete = true };
                IsBusy = true;
                bool result = await ProductService.UpdateDeleteProduct(arguments);
                IsBusy = false;
                if (result)
                {
                    Products.RemoveAt(ItemIndex);
                }
                return result;
            }
            return false;
        }
    }
}