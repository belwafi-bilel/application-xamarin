using Plugin.Messaging;
using RealEstate.Mobile.Extensions;
using RealEstate.Mobile.Models;
using RealEstate.Mobile.Services;
using RealEstate.Models.API.RealEstate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RealEstate.Mobile.ViewModels
{
    public class ProductViewViewModel : BaseViewModel
    {
        public ProductService ProductService => new ProductService();
        public Command<string> CallCommand { get; set; }

        public ProductViewViewModel()
        {
            CallCommand = new Command<string>(Call);
        }
        private async Task<EstateResultDto> GetProducts(ProductArguments productArguments)
        {
            return await ProductService.GetProducts(productArguments);
        }
        private void Call(string mobileNumber)
        {
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall(mobileNumber);
        }

        public ProductItemViewModel ProductItemViewModel { get; set; }
        public void BindExistingData()
        {
            IsBusy = true;
            if (ProductItemViewModel != null)
            {
                Town = ProductItemViewModel.TownName;
                ListingType = ProductItemViewModel.ListingType;
                ProductName = ProductItemViewModel.ProductName;
                IsHot = ProductItemViewModel.IsHot;
                Area = ProductItemViewModel.Area;
                DateCreate = ProductItemViewModel.CreatedDate;
                DateUpdate = ProductItemViewModel.UpdatedDate;
                HouseNumber = ProductItemViewModel.HouseNumber;
                StaffMobile = ProductItemViewModel.StaffInfo.Mobile;
                StaffName = ProductItemViewModel.StaffInfo.Name;
                ListingType = ProductItemViewModel.ListingName;
                SalePrice = (float?)(ProductItemViewModel.SalePrice);
                SaleUnit = ProductItemViewModel.SaleUnit;
                RentPrice = (float?)ProductItemViewModel.RentPrice;
                RentUnit = ProductItemViewModel.RentUnit;
                Mobile = ProductItemViewModel.OwnerInfo.Mobile;
                OwnerName = ProductItemViewModel.OwnerInfo.Name;
                Notes = ProductItemViewModel.Notes?.TrimStart()?.TrimEnd();
                if(ProductItemViewModel.ImageUrls != null && ProductItemViewModel.ImageUrls.Any())
                {
                    Photos = new ObservableCollection<string>(ProductItemViewModel.ImageUrls.Select(x => x.Url));
                }
            }
            IsBusy = false;
        }
        public ObservableCollection<string> _photos = new ObservableCollection<string>();
        public ObservableCollection<string> Photos
        {
            get { return _photos; }
            set { SetProperty(ref _photos, value); }
        }
        public GooglePlaceAutoCompletePrediction SelectedPlace;

        private bool _ShowOwnerName = true;
        public bool ShowOwnerName
        {
            get { return _ShowOwnerName; }
            set { SetProperty(ref _ShowOwnerName, value); }
        }

        private bool _allowEditProductCode = true;
        public bool AllowEditProductCode
        {
            get { return _allowEditProductCode; }
            set { SetProperty(ref _allowEditProductCode, value); }
        }
        private bool _allowEditHouseNumber = true;
        public bool AllowEditHouseNumber
        {
            get { return _allowEditHouseNumber; }
            set { SetProperty(ref _allowEditHouseNumber, value); }
        }

        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set { SetProperty(ref _productName, value); }
        }
        private bool? _isHot;
        public bool? IsHot
        {
            get { return _isHot; }
            set { SetProperty(ref _isHot, value); }
        }
        private string _area;
        public string Area
        {
            get { return _area; }
            set { SetProperty(ref _area, value); }
        }
        private float? _salePrice;
        public float? SalePrice
        {
            get { return _salePrice; }
            set { SetProperty(ref _salePrice, value); }
        }
        private float? _rentPrice;
        public float? RentPrice
        {
            get { return _rentPrice; }
            set { SetProperty(ref _rentPrice, value); }
        }
        private string _houseNumber;
        public string HouseNumber
        {
            get { return _houseNumber; }
            set { SetProperty(ref _houseNumber, value); }
        }
        private string _mobile;
        public string Mobile
        {
            get { return _mobile; }
            set { SetProperty(ref _mobile, value); }
        }
        private string _ownerName;
        public string OwnerName
        {
            get { return _ownerName; }
            set { SetProperty(ref _ownerName, value); }
        }

        private string _staffMobile;
        public string StaffMobile
        {
            get { return _staffMobile; }
            set { SetProperty(ref _staffMobile, value); }
        }
        private string _staffName;
        public string StaffName
        {
            get { return _staffName; }
            set { SetProperty(ref _staffName, value); }
        }

        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        private string _listingType;
        public string ListingType
        {
            get { return _listingType; }
            set { SetProperty(ref _listingType, value); }
        }
        private string _saleUnit;
        public string SaleUnit
        {
            get { return _saleUnit; }
            set { SetProperty(ref _saleUnit, value); }
        }
        private string _rentUnit;
        public string RentUnit
        {
            get { return _rentUnit; }
            set { SetProperty(ref _rentUnit, value); }
        }

        private string _town;
        public string Town
        {
            get { return _town; }
            set { SetProperty(ref _town, value); }
        }

        private DateTime? _dateCreated;
        public DateTime? DateCreate
        {
            get { return _dateCreated; }
            set { SetProperty(ref _dateCreated, value); }
        }

        private DateTime? _dateUpdated;
        public DateTime? DateUpdate
        {
            get { return _dateUpdated; }
            set { SetProperty(ref _dateUpdated, value); }
        }
    }
}
