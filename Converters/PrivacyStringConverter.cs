using System.Globalization;
using System.Windows.Data;

public class PrivacyStringConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[1] is int jePrivate && jePrivate == 1)
        {
            return "***";
        }
        return values[0]?.ToString() ?? string.Empty;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
