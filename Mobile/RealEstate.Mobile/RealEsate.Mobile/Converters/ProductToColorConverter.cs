using System;
using System.Globalization;
using RealEstate.Mobile.ViewModels;
using Xamarin.Forms;
using Type = System.Type;

namespace RealEstate.Mobile.Converters
{
    public class ProductToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isDeleted = (bool)value;
            if (isDeleted)
                return "#ff0000";
            return "#ffffff";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
