using System;
using System.Globalization;
using System.Windows.Data;
using DopravniPodnikSem.Models.Enum;

namespace DopravniPodnikSem.Converters
{
    public class RoleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Role role)
            {
                switch (role)
                {
                    case Role.Administrator:
                        return "Administrator";
                    case Role.Zamestnanec:
                        return "Zamestnanec";
                    case Role.Guest:
                        return "Guest";
                    default:
                        return "Unknown";
                }
            }

            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
