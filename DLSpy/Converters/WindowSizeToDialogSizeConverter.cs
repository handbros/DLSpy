using System;
using System.Globalization;
using System.Windows.Data;

namespace DLSpy.Converters
{
    /// <summary>
    /// Window의 사이즈를 Parameter 값을 통해 Material Design Dialog의 사이즈로 변환합니다.
    /// </summary>
    [ValueConversion(typeof(double), typeof(double))]
    public class WindowSizeToDialogSizeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - double.Parse((string)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value + double.Parse((string)parameter);
        }

        #endregion
    }
}
