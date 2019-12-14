using System;
using System.Globalization;
using Xamarin.Forms;
namespace RealEstate.Mobile.Converters
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || System.Convert.ToInt32(value) == 0)
                return null;
            else
                return double.Parse(value.ToString().Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.ToString() == "")
                return 0;
            else
            {
                var price = double.Parse(value.ToString().Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
                return price;
            }
        }
    }
}
