using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DopravniPodnikSem.Converters
{
    public class WidthAdjustConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double actualWidth)
            {
                double factor = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
                return actualWidth * factor;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
