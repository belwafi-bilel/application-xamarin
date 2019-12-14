using RealEstate.Models.API.RealEstate;
using System;
using System.Collections.Generic;

namespace RealEstate.Mobile.ViewModels
{
    public class ProductItemViewModel :BaseViewModel
    {
        public long Id { get; set; }

        private string _productName;
        public string ProductName 
        {
            get { return _productName; }
            set { SetProperty(ref _productName, value); }
        }
        public double? SalePrice { get; set; }
        public long SaleUnitId { get; set; }
        public string SaleUnit { get; set; }
        public double? RentPrice { get; set; }
        public long RentUnitId { get; set; }
        public string RentUnit { get; set; }
        public long TownId { get; set; }
        public long ListingTypeId { get; set; }
        public string ListingType { get; set; }
        public string ListingName { get; set; }
        public string TownName { get; set; }
        public string HouseNumber { get; set; }
        public StaffInfoViewModel StaffInfo { get; set; }
        public OwnerInfoViewModel OwnerInfo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Area { get; set; }
        public string Notes { get; set; }
        public bool IsDeleted { get; set; }

        private bool _isHot;
        public bool IsHot
        {
            get { return _isHot; }
            set { SetProperty(ref _isHot, value); }
        }

        public int CreatorId { get; set; }
        public bool ShowOwnerInfo => CreatorId == App.AccountInfo.Id;
        public bool AllowDelete => CreatorId == App.AccountInfo.Id;
        public bool ShowDeleteHeader => IsDeleted == true;
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public string MainPinText { get; set; }
        public string Address { get; set; }
        public List<EstateImageDto> ImageUrls { get; set; }

    }
    public class StaffInfoViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
    }
    public class OwnerInfoViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
    }
}
