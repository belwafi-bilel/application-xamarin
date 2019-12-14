using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RealEstate.Mobile.Extensions;
using RealEstate.Mobile.Helpers;
using RealEstate.Mobile.Models;
using RealEstate.Mobile.Services;
using RealEstate.Models.API.RealEstate;
using Xamarin.Forms;

namespace RealEstate.Mobile.ViewModels
{
    public class ProductDetailViewModel : BaseViewModel
    {
        public ProductService ProductService => new ProductService();
        public DialogService DialogService => new DialogService();
        public GoogleMapsApiService GoogleMapService => new GoogleMapsApiService();
        public ProductDetailViewModel()
        {
        }
        public async Task<bool> SaveProductAsync()
        {
            var product = new RealEstateEstateDto
            {
                Estate_TypeId = SelectedListingType?.Id,
                Town = new RealEstateTownDto { TownId = SelectedTown?.Id },
                EstateCode = ProductName,
                IsHot = IsHot,
                Account = new RealEstateAccountDto { AccountId = App.AccountInfo.Id },
                Area = Area,
                HouseNumber = HouseNumber,
                SalePrice = SalePrice,
                SaleUnit = new RealEstateSaleUnitDto { SaleUnitId = SelectedSaleUnit?.Id },
                RentPrice = RentPrice,
                RentUnit = new RealEstateRentUnitDto { RentUnitId = SelectedRentUnit?.Id },
                Lat = SelectedPlace?.Lat,
                Long = SelectedPlace?.Long,
                MainPinText = SelectedPlace?.StructuredFormatting?.MainText,
                Address = SelectedPlace?.Description,
                Phone = Mobile,
                OwnerName = OwnerName,
                Note = Notes
            };
            if(ProductItemViewModel == null)
            {
                if(Photos.Any())
                {
                    foreach(var item in Photos.Where(x => x.Id == 0))
                    {
                        var returnId = await ProductService.UploadMedias(item.ImageName, await item.ImageSource.GetStreamFromImageSource());
                        if (returnId.HasValue)
                            product.EstateImageIds.Add(returnId.Value);
                        else
                        {
                            await DialogService.ShowAlertAsync("Can't upload medias to server. Please retry later", "Warning", "Ok");
                            return false;
                        }
                    }
                    
                }
                return await ProductService.CreateProduct(product);
            }
            else
            {
                product.Id = ProductItemViewModel.Id;
                return await ProductService.UpdateProduct(product);
            }
        }
       
        public bool ValidateForm()
        {
            IsTownInValid = SelectedTown == null;
            IsListingInValid = SelectedListingType == null;
            IsProductCodeInValid = string.IsNullOrEmpty(ProductName);
            IsPriceInValid = SalePrice == 0 && RentPrice == 0;

            IsSaleUnitInValid = (SalePrice != 0 && SelectedSaleUnit == null) ? true : false;

            IsRentUnitInValid = (RentPrice != 0 && SelectedRentUnit == null) ? true : false;
            if(RentPrice == 0)
            {
                IsRentUnitInValid = false;
            }
            if (SalePrice == 0)
            {
                IsSaleUnitInValid = false;
            }
            IsOwnerMobileInValid = string.IsNullOrEmpty(Mobile);

            if(IsTownInValid || IsListingInValid || IsProductCodeInValid || IsPriceInValid
                || IsSaleUnitInValid || IsRentUnitInValid || IsOwnerMobileInValid)
            {
                return false;
            }
            return true;
        }
        public async Task LoadData()
        {
            try
            {
                var result = await ProductService.GetMasterData();
                //var towns = await ProductService.GetTowns();
                if (result.MasterData.Towns != null)
                {
                    Towns = new ObservableCollection<TownViewModel>(result.MasterData.Towns.Select(x => new TownViewModel
                    {
                        Id = x.TownId ?? 0,
                        Name = x.Name
                    }));
                }

                //var listingTypes = await ProductService.GetListingTypes();
                if (result.MasterData.ListingTypes != null)
                {
                    ListingTypes = new ObservableCollection<ListingTypeViewModel>(result.MasterData.ListingTypes.Select(x => new ListingTypeViewModel
                    {
                        Id = x.Estate_TypeId ?? 0,
                        Name = x.Name
                    }));
                }
                //var saleUnits = await ProductService.GetSaleUnits();
                if (result.MasterData.SaleUnits != null)
                {
                    SaleUnits = new ObservableCollection<SaleUnitViewModel>(result.MasterData.SaleUnits.Select(x => new SaleUnitViewModel
                    {
                        Id = x.SaleUnitId ?? 0,
                        Name = x.Name
                    }));
                    SelectedSaleUnit = SaleUnits.FirstOrDefault(x => x.Id == 2);
                }
                //var rentUnits = await ProductService.GetRentUnits();
                if (result.MasterData.RentUnits != null)
                {
                    RentUnits = new ObservableCollection<RentUnitViewModel>(result.MasterData.RentUnits.Select(x => new RentUnitViewModel
                    {
                        Id = x.RentUnitId ?? 0,
                        Name = x.Name
                    }));
                    SelectedRentUnit = RentUnits.FirstOrDefault(x => x.Id == 2);
                }
            }
            catch(Exception ex)
            {

            }
            
        }
        public ProductItemViewModel ProductItemViewModel { get; set; }
        public void BindExistingData()
        {
            if (ProductItemViewModel != null)
            {
                Title = "Edit Product";
                SelectedTown = Towns.FirstOrDefault(x => x.Id == ProductItemViewModel.TownId);
                SelectedListingType = ListingTypes.FirstOrDefault(x => x.Id == ProductItemViewModel.ListingTypeId);
                ProductName = ProductItemViewModel.ProductName;
                IsHot = ProductItemViewModel.IsHot;
                Area = ProductItemViewModel.Area;
                HouseNumber = ProductItemViewModel.HouseNumber;
                SalePrice = (float)ProductItemViewModel.SalePrice;
                SelectedSaleUnit = SaleUnits.FirstOrDefault(x => x.Id == ProductItemViewModel.SaleUnitId);
                RentPrice = (float)ProductItemViewModel.RentPrice;
                SelectedRentUnit = RentUnits.FirstOrDefault(x => x.Id == ProductItemViewModel.RentUnitId);
                Mobile = ProductItemViewModel.OwnerInfo.Mobile;
                OwnerName = ProductItemViewModel.OwnerInfo.Name;
                Notes = ProductItemViewModel.Notes;
                if(ProductItemViewModel.ImageUrls != null && ProductItemViewModel.ImageUrls.Any())
                {
                    Photos = new ObservableCollection<ImageItemViewModel>(ProductItemViewModel.ImageUrls.Select(x => new ImageItemViewModel
                    {
                        Id = x.Id,
                        ImageName = x.Name,
                        ImageSource = new UriImageSource { Uri = new Uri(x.Url) }
                    }));
                }
            }
            else
                Title = "Add Product";
        }
        
        public async Task OpenCamearaAsync()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);             if (cameraStatus != PermissionStatus.Granted)             {                 var results = await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Camera);                 if (results.ContainsKey(Plugin.Permissions.Abstractions.Permission.Camera))                     cameraStatus = results[Plugin.Permissions.Abstractions.Permission.Camera];             }             var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);             if (storageStatus != PermissionStatus.Granted)             {                 var results = await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Storage);                 if (results.ContainsKey(Plugin.Permissions.Abstractions.Permission.Storage))                     storageStatus = results[Plugin.Permissions.Abstractions.Permission.Storage];             }              if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DialogService.ShowAlertAsync("Camera is not ready", "Warning", "Ok");
                    return;
                }
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                    CompressionQuality = 70
                });

                if (file == null)
                    return;
                var imageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
                Photos.Add(new ImageItemViewModel
                {
                    ImageSource = imageSource,
                    ImageName = file.Path.Substring(file.Path.LastIndexOf("/") + 1)
                });
            }
        }
        public async Task BrowseGalleryAsync()
        {
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
            if (storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Storage);
                if (results.ContainsKey(Plugin.Permissions.Abstractions.Permission.Storage))
                    storageStatus = results[Plugin.Permissions.Abstractions.Permission.Storage];
            }

            if (storageStatus == PermissionStatus.Granted)
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    return;
                }
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    SaveMetaData = false,
                    CompressionQuality = 30,
                    MaxWidthHeight = 1024
                });
                if (file != null)
                {
                    var imageSource = ImageSource.FromFile(file.Path);
                    Photos.Add(new ImageItemViewModel
                    {
                        ImageSource = imageSource,
                        ImageName = file.Path.Substring(file.Path.LastIndexOf("/") + 1)
                    });
                }
            }
        }
        public GooglePlaceAutoCompletePrediction SelectedPlace;
        private ObservableCollection<GooglePlaceAutoCompletePrediction> _places = new ObservableCollection<GooglePlaceAutoCompletePrediction>();
        public ObservableCollection<GooglePlaceAutoCompletePrediction> Places
        {
            get { return _places; }
            set { SetProperty(ref _places, value); }
        }
        public async Task GetPlaces(string place)
        {
            GooglePlaceAutoCompleteResult result = await GoogleMapService.GetPlaces(place);
            if(result != null && result.Status == "OK")
            {
                Places = new ObservableCollection<GooglePlaceAutoCompletePrediction>(result.AutoCompletePlaces);
            }
        }
        public async Task<GooglePlace> GetPlaceDetails(string placeId)
        {
            return await GoogleMapService.GetPlaceDetails(placeId);
        }
        private ObservableCollection<ImageItemViewModel> _photos = new ObservableCollection<ImageItemViewModel>();
        public ObservableCollection<ImageItemViewModel> Photos
        {
            get { return _photos; }
            set { SetProperty(ref _photos, value); }
        }

        private bool _showOwnerName = true;
        public bool ShowOwnerName
        {
            get { return _showOwnerName; }
            set { SetProperty(ref _showOwnerName, value); }
        }
        private bool _showButtonSave = true;
        public bool ShowButtonSave
        {
            get { return _showButtonSave; }
            set { SetProperty(ref _showButtonSave, value); }
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
        private bool _allowEditOwnerNumber = true;
        public bool AllowEditOwnerNumber
        {
            get { return _allowEditOwnerNumber; }
            set { SetProperty(ref _allowEditOwnerNumber, value); }
        }
        private bool _allowEditTown = true;
        public bool AllowEditTown
        {
            get { return _allowEditTown; }
            set { SetProperty(ref _allowEditTown, value); }
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
        private float _salePrice;
        public float SalePrice
        {
            get { return _salePrice; }
            set { SetProperty(ref _salePrice, value); }
        }
        private float _rentPrice;
        public float RentPrice
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
        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
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

        private TownViewModel _selectedTown;
        public TownViewModel SelectedTown
        {
            get { return _selectedTown; }
            set { SetProperty(ref _selectedTown, value); }
        }

        private ListingTypeViewModel _selectedListingType;
        public ListingTypeViewModel SelectedListingType
        {
            get { return _selectedListingType; }
            set { SetProperty(ref _selectedListingType, value); }
        }
        private SaleUnitViewModel _selectedSaleUnit;
        public SaleUnitViewModel SelectedSaleUnit
        {
            get { return _selectedSaleUnit; }
            set { SetProperty(ref _selectedSaleUnit, value); }
        }
        private RentUnitViewModel _selectedRentUnit;
        public RentUnitViewModel SelectedRentUnit
        {
            get { return _selectedRentUnit; }
            set { SetProperty(ref _selectedRentUnit, value); }
        }
        private bool _isTownInValid;
        public bool IsTownInValid
        {
            get { return _isTownInValid; }
            set { SetProperty(ref _isTownInValid, value); }
        }
        private bool _isListingInValid;
        public bool IsListingInValid
        {
            get { return _isListingInValid; }
            set { SetProperty(ref _isListingInValid, value); }
        }
        private bool _isProductCodeInValid;
        public bool IsProductCodeInValid
        {
            get { return _isProductCodeInValid; }
            set { SetProperty(ref _isProductCodeInValid, value); }
        }
        private bool _isPriceInValid;
        public bool IsPriceInValid
        {
            get { return _isPriceInValid; }
            set { SetProperty(ref _isPriceInValid, value); }
        }
        private bool _isOwnerMobileInValid;
        public bool IsOwnerMobileInValid
        {
            get { return _isOwnerMobileInValid; }
            set { SetProperty(ref _isOwnerMobileInValid, value); }
        }
        private bool _isSaleUnitInValid;
        public bool IsSaleUnitInValid
        {
            get { return _isSaleUnitInValid; }
            set { SetProperty(ref _isSaleUnitInValid, value); }
        }
        private bool _isRentUnitInValid;
        public bool IsRentUnitInValid
        {
            get { return _isRentUnitInValid; }
            set { SetProperty(ref _isRentUnitInValid, value); }
        }
        public long Id { get; set; }
    }
}
