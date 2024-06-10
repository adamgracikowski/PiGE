using System.Globalization;
using System.Windows.Data;

namespace AILabelTool
{
    public class NextPreviousConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] is not int index || values[1] is not int count)
                return false;
            return parameter.ToString() switch
            {
                "Previous" => index > 0,
                "Next" => index < count - 1,
                _ => false
            };
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) 
        { 
            throw new NotImplementedException();
        }
    }
}
