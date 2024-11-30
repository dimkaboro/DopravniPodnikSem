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
                        return "Вы вошли как Админ";
                    case Role.Zamestnanec:
                        return "Вы вошли как Сотрудник";
                    case Role.Guest:
                        return "Вы вошли как Гость";
                    default:
                        return "Роль не определена";
                }
            }

            return "Роль не определена";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
