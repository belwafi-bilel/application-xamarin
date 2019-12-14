using RealEstate.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace RealEstate.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductViewPage : ContentPage
	{
        ProductViewViewModel viewModel;
        private ProductViewViewModel _context => (ProductViewViewModel)this.BindingContext;

        public ProductViewPage(ProductItemViewModel productItemViewModel)
        {
            InitializeComponent();
            viewModel = new ProductViewViewModel();
            BindingContext = viewModel;
            viewModel.ProductItemViewModel = productItemViewModel;
            if (productItemViewModel != null)
            {
                if (productItemViewModel.CreatorId != App.AccountInfo.Id)
                    viewModel.ShowOwnerName = false;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.BindExistingData();
            if(_context.ProductItemViewModel.Lat.HasValue && _context.ProductItemViewModel.Long.HasValue)
            {
                var pin = new Pin()
                {
                    Type = PinType.Place,
                    Icon = BitmapDescriptorFactory.FromBundle("estate_pin.png"),
                    Label = _context.ProductItemViewModel?.MainPinText,
                    Address = _context.ProductItemViewModel?.Address,
                    Position = new Position(_context.ProductItemViewModel.Lat.Value, _context.ProductItemViewModel.Long.Value),
                    IsVisible = true
                };

                map.Pins.Add(pin);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(500)));
                map.UiSettings.MapToolbarEnabled = true;
            }
            else
            {
                map.IsVisible = false;
            }
        }
    }
}