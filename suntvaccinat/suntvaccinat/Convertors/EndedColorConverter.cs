using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace suntvaccinat.Convertors
{
    public class EndedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                bool val = (bool)value;

                if (val == false)
                {
                    Color col = Color.Tan;
                    return Color.FromHex("#8e9c8b");
                }

            }
            return Color.Tan;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
