using RealEstate.Mobile.Extensions;
using RealEstate.Mobile.Services;
using RealEstate.Models.API.RealEstate;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace RealEstate.Mobile.ViewModels
{
    public class ProductCategoryViewModel : BaseViewModel
    {
        private ProductService ProductService => new ProductService();
        public ProductCategoryViewModel()
        {
            Title = "Estate Types";
        }

        public async Task LoadData()
        {
            IsRefreshing = true;
            var listingTypes = await ProductService.GetListingTypes();
            if (listingTypes != null && listingTypes.Estate_TypeResults != null)
            {
                ListingTypes = listingTypes.Estate_TypeResults.Select(x => new ListingTypeViewModel
                {
                    Id = x.Estate_TypeId ?? 0,
                    Name = x.Name,
                    TotalProduct = x.TotalProduct
                }).ToObservableCollection();
            }
            IsRefreshing = false;
        }

        private ObservableCollection<ListingTypeViewModel> _listingTypes = new ObservableCollection<ListingTypeViewModel>();
        public ObservableCollection<ListingTypeViewModel> ListingTypes
        {
            get { return _listingTypes; }
            set { SetProperty(ref _listingTypes, value); }
        }
        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }
    }
}