using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DopravniPodnikSem.Converters
{
    public class StepToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int currentStep = (int)value;
            int targetStep = int.Parse(parameter.ToString());

            return currentStep == targetStep ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
