using System;
using System.Globalization;
using RealEstate.Mobile.ViewModels;
using Xamarin.Forms;

namespace RealEstate.Mobile.Converters
{
    public class HouseNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            var product = (ProductItemViewModel)value;
            if (!string.IsNullOrEmpty(product.HouseNumber))
                return $"SN: ({product.HouseNumber})";
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value?.ToString()).Replace("SN: ", "").Replace("(", "").Replace(")", "");
        }
    }
}
