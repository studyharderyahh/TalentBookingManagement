using System;
using System.Globalization;
using System.Windows.Data;

namespace TalentBookingManagement.Converters
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (decimal.TryParse((string)value, out decimal result))
            {
                return result;
            }
            return 0;
        }
    }
}
