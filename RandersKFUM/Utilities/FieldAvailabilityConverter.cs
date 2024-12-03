using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RandersKFUM.Utilities
{
    public class FieldAvailabilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int fieldId && values[1] is BookingViewModel viewModel)
            {
                return viewModel.FieldAvailability.TryGetValue(fieldId, out bool isAvailable) && isAvailable ? Brushes.Green : Brushes.Red;
            }
            return Brushes.Gray; // Returner Gray hvis der er en fejl eller data mangler
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Konvertering tilbage er ikke implementeret og ikke nødvendig her.");
        }
    }
}