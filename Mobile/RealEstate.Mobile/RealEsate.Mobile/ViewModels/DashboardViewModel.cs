using RealEstate.Mobile.AppSettings;
using RealEstate.Mobile.Extensions;
using RealEstate.Mobile.Services;
using RealEstate.Models.API.RealEstate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mobile.ViewModels
{
    public class DashboardViewModel: BaseViewModel
    {
        public ProductService ProductService => new ProductService();

        public async Task LoadData()
        {
            var towns = await ProductService.GetTowns();
            if (towns != null && towns.TownResults != null)
            {
                Towns = towns.TownResults.Select(x => new TownViewModel
                {
                    Id = x.TownId ?? 0,
                    Name = x.Name
                }).ToObservableCollection();
            }

            var listingTypes = await ProductService.GetListingTypes();
            if (listingTypes != null && listingTypes.Estate_TypeResults != null)
            {
                ListingTypes = listingTypes.Estate_TypeResults.Select(x => new ListingTypeViewModel
                {
                    Id = x.Estate_TypeId ?? 0,
                    Name = x.Name
                }).ToObservableCollection();
            }

            var saleUnits = await ProductService.GetSaleUnits();
            if (saleUnits != null && saleUnits.SaleUnitResults != null)
            {
                SaleUnits = saleUnits.SaleUnitResults.Select(x => new SaleUnitViewModel
                {
                    Id = x.SaleUnitId ?? 0,
                    Name = x.Name
                }).ToObservableCollection();
            }
            var rentUnits = await ProductService.GetRentUnits();
            if (rentUnits != null && rentUnits.RentUnitResults != null)
            {
                RentUnits = rentUnits.RentUnitResults.Select(x => new RentUnitViewModel
                {
                    Id = x.RentUnitId ?? 0,
                    Name = x.Name
                }).ToObservableCollection();
            }
            var dashBoard = await ProductService.GetDashBoard();
            if (dashBoard  != null)
            {
                DashBoard = dashBoard;               
            }
        }

        private ObservableCollection<TownViewModel> _towns = new ObservableCollection<TownViewModel>();
        public ObservableCollection<TownViewModel> Towns
        {
            get { return _towns; }
            set { SetProperty(ref _towns, value); }
        }
        private ObservableCollection<ListingTypeViewModel> _listingTypes = new ObservableCollection<ListingTypeViewModel>();
        public ObservableCollection<ListingTypeViewModel> ListingTypes
        {
            get { return _listingTypes; }
            set { SetProperty(ref _listingTypes, value); }
        }
        private ObservableCollection<SaleUnitViewModel> _saleUnits = new ObservableCollection<SaleUnitViewModel>();
        public ObservableCollection<SaleUnitViewModel> SaleUnits
        {
            get { return _saleUnits; }
            set { SetProperty(ref _saleUnits, value); }
        }
        private ObservableCollection<RentUnitViewModel> _rentUnits = new ObservableCollection<RentUnitViewModel>();
        public ObservableCollection<RentUnitViewModel> RentUnits
        {
            get { return _rentUnits; }
            set { SetProperty(ref _rentUnits, value); }
        }
        private RealEstateDashboardDto _dashBoard = new RealEstateDashboardDto();
        public RealEstateDashboardDto DashBoard
        {
            get { return _dashBoard; }
            set { SetProperty(ref _dashBoard, value); }
        }
        private ObservableCollection<TypeViewModel> _rentalTypes = new ObservableCollection<TypeViewModel>(Settings.Types);
        public ObservableCollection<TypeViewModel> RentalTypes
        {
            get { return _rentalTypes; }
            set { SetProperty(ref _rentalTypes, value); }
        }

        private ObservableCollection<PriceViewModel> _salePriceRanges = new ObservableCollection<PriceViewModel>(Settings.SalePrices);
        public ObservableCollection<PriceViewModel> SalePriceRanges
        {
            get { return _salePriceRanges; }
            set { SetProperty(ref _salePriceRanges, value); }
        }

        private ObservableCollection<PriceViewModel> _rentPriceRanges = new ObservableCollection<PriceViewModel>(Settings.RentPrices);
        public ObservableCollection<PriceViewModel> RentPriceRanges
        {
            get { return _rentPriceRanges; }
            set { SetProperty(ref _rentPriceRanges, value); }
        }

        private ObservableCollection<AreaViewModel> _areaRanges = new ObservableCollection<AreaViewModel>(Settings.Areas);
        public ObservableCollection<AreaViewModel> AreaRanges
        {
            get { return _areaRanges; }
            set { SetProperty(ref _areaRanges, value); }
        }
    }
}
