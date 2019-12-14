using RealEstate.Mobile.Services;
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
	public partial class HelpPage : ContentPage
	{
        HelpViewModel viewModel;
        public HelpPage ()
		{
			InitializeComponent ();
            viewModel = new HelpViewModel();
            BindingContext = viewModel;
        }
        private void NotifyItem_Clicked(object sender, EventArgs e)
        {

        }
    }
}