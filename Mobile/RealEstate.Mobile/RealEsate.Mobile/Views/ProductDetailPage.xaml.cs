using RealEstate.Mobile.Models;
using RealEstate.Mobile.Services;
using RealEstate.Mobile.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace RealEstate.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailPage : ContentPage
    {
        private ProductDetailViewModel viewModel;
        private DialogService dialogService;
        private bool _isFromProductPage = true;
        private ProductDetailViewModel _context => (ProductDetailViewModel)BindingContext;

        public ProductDetailPage(ProductItemViewModel productItemViewModel = null, bool isFromProductPage = true)
        {
            try
            {
                InitializeComponent();
                map.PinDragEnd += Map_PinDragEnd;
            }
            catch(Exception ex)
            {

            }
            
            viewModel = new ProductDetailViewModel();
            _isFromProductPage = isFromProductPage;
            BindingContext = viewModel;
            dialogService = new DialogService();
            viewModel.ProductItemViewModel = productItemViewModel;
            if(productItemViewModel != null)
            {
                viewModel.AllowEditProductCode = false;
                viewModel.AllowEditHouseNumber = false;
                viewModel.AllowEditOwnerNumber = false;
                viewModel.AllowEditTown = false;
                if (productItemViewModel.CreatorId != App.AccountInfo.Id)
                    viewModel.ShowOwnerName = false;
            }
        }

        private void Map_PinDragEnd(object sender, PinDragEventArgs e)
        {
            
        }
        private string PrintPin(Pin pin)
        {
            return $"{pin.Label}({pin.Position.Latitude.ToString("0.000")},{pin.Position.Longitude.ToString("0.000")})";
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            Task.Run(async () =>
            {
                viewModel.IsBusy = true;
                await viewModel.LoadData();
                viewModel.BindExistingData();
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (viewModel.IsHot ?? false)
                        RadioButtonYes.IsChecked = true;
                    else
                        RadioButtonNo.IsChecked = true;
                });
                if (viewModel.ProductItemViewModel != null && viewModel.ProductItemViewModel.Lat.HasValue
                    && viewModel.ProductItemViewModel.Long.HasValue)
                {
                    _currentPin = new Pin()
                    {
                        Type = PinType.Place,
                        Icon = BitmapDescriptorFactory.FromBundle("estate_pin.png"),
                        Label = viewModel.ProductItemViewModel?.MainPinText,
                        Address = viewModel.ProductItemViewModel?.Address,
                        Position = new Position(viewModel.ProductItemViewModel.Lat.Value, viewModel.ProductItemViewModel.Long.Value),
                        Rotation = 0f,
                        IsVisible = true,
                        IsDraggable = true
                    };

                    map.Pins.Add(_currentPin);
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(_currentPin.Position, Distance.FromMeters(500)));
                    map.UiSettings.MapToolbarEnabled = true;
                }
                viewModel.IsBusy = false;
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
        }
        async void OnSave_Clicked(object sender, System.EventArgs e)
        {
            bool isValid = viewModel.ValidateForm();
            if (!isValid)
                return;

            viewModel.IsBusy = true;
            bool result = await viewModel.SaveProductAsync();
            viewModel.IsBusy = false;
            if (result)
            {
               await Navigation.PopAsync();
            }
        }

        void RadioYes_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (e.IsChecked ?? false)
            {
                viewModel.IsHot = true;
            }
        }
        void RadioNo_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (e.IsChecked ?? false)
            {
                viewModel.IsHot = false;
            }
        }
        void OnTowns_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            viewModel.SelectedTown = e.Value as TownViewModel;
        }

        void OnListingTypes_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            viewModel.SelectedListingType = e.Value as ListingTypeViewModel;
        }

        void OnSaleUnits_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            viewModel.SelectedSaleUnit = e.Value as SaleUnitViewModel;
        }

        void OnRentUnits_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            viewModel.SelectedRentUnit = e.Value as RentUnitViewModel;
        }

        private async void OnCameraSfButton_Clicked(object sender, EventArgs e)
        {
            await _context.OpenCamearaAsync();
        }
        private async void OnPhotosSfButton_Clicked(object sender, EventArgs e)
        {
            await _context.BrowseGalleryAsync();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            MediaPopup.IsVisible = true;
            MediaPopup.Show();
        }
        private async void OnCameraSfButton_Tapped(object sender, EventArgs e)
        {
            MediaPopup.IsVisible = false;
            MediaPopup.Dismiss();
            await _context.OpenCamearaAsync();
        }
        private async void OnPhotosSfButton_Tapped(object sender, EventArgs e)
        {
            MediaPopup.IsVisible = false;
            MediaPopup.Dismiss();
            await _context.BrowseGalleryAsync();
        }

        private async void AutoComplete_ValueChanged(object sender, Syncfusion.SfAutoComplete.XForms.ValueChangedEventArgs e)
        {
            if(e.Value != null && e.Value.Length > 3)
            {
                await _context.GetPlaces(e.Value);
            }
        }
        Pin _currentPin;
        private async void AutoComplete_SelectionChanged(object sender, Syncfusion.SfAutoComplete.XForms.SelectionChangedEventArgs e)
        {
            if(e.Value != null && e.Value is GooglePlaceAutoCompletePrediction selectedPlace)
            {
                var placeDetails = await _context.GetPlaceDetails(selectedPlace.PlaceId);
                if(placeDetails != null)
                {
                    selectedPlace.Lat = placeDetails.Latitude;
                    selectedPlace.Long = placeDetails.Longitude;
                    _context.SelectedPlace = selectedPlace;
                    if (_currentPin != null)
                        map.Pins.Remove(_currentPin);

                    _currentPin = new Pin()
                    {
                        Type = PinType.Place,
                        Icon = BitmapDescriptorFactory.FromBundle("estate_pin.png"),
                        Label = selectedPlace.StructuredFormatting?.MainText,
                        Address = selectedPlace.Description,
                        Position = new Position(placeDetails.Latitude, placeDetails.Longitude),
                        Rotation = 0f,
                        IsVisible = true,
                        IsDraggable = true
                    };
                    
                    map.Pins.Add(_currentPin);
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(_currentPin.Position, Distance.FromMeters(500)));
                    map.UiSettings.MapToolbarEnabled = true;
                }
            }
        }
    }
}