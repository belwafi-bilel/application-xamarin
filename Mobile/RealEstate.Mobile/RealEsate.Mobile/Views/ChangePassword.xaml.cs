using Plugin.FirebasePushNotification;
using RealEstate.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealEstate.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePassword : ContentPage
    {
        ChangePasswordViewModel viewModel;
        public ChangePassword()
        {
            InitializeComponent();
            BindingContext = viewModel = new ChangePasswordViewModel();
        }
    }
}