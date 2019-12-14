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
    public class LocationViewModel : BaseViewModel
    {
        private ProductService ProductService => new ProductService();
        public async Task LoadData()
        {
            IsBusy = true;
            await SendRequest();
            IsBusy = false;
        }
        public async Task RefreshData()
        {
            IsRefreshing = true;
            await SendRequest();
            IsRefreshing = false;
        }
        private async Task SendRequest()
        {
            var towns = await ProductService.GetTowns();
            if (towns != null && towns.TownResults != null)
            {
                Towns = towns.TownResults.Select(x => new TownViewModel
                {
                    Id = x.TownId ?? 0,
                    Name = x.Name,
                    TotalProduct = x.TotalProduct
                }).ToObservableCollection();
            }
        }
        private ObservableCollection<TownViewModel> _towns = new ObservableCollection<TownViewModel>();
        public ObservableCollection<TownViewModel> Towns
        {
            get { return _towns; }
            set { SetProperty(ref _towns, value); }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }
    }    
}
