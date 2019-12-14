using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RealEstate.Mobile.ViewModels
{
    public class ImageItemViewModel
    {
        public long Id { get; set; }
        public ImageSource ImageSource { get; set; }
        public string ImageName { get; set; }
    }
}
