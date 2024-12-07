using System;
using System.Globalization;
using System.Windows.Data;

namespace DopravniPodnikSem.Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Преобразование int -> bool (1 -> true, 0 -> false)
            if (value is int intValue)
                return intValue == 1;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Преобразование bool -> int (true -> 1, false -> 0)
            if (value is bool boolValue)
                return boolValue ? 1 : 0;
            return 0;
        }
    }
}
