using System;
using System.Globalization;
using CommonShared.DataModels;
using Xamarin.Forms;

namespace AppShared.Converters
{
    public class ExcuseStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ExcuseState excuseState)
            {
                return null;
            }

            return excuseState switch
            {
                ExcuseState.Passed => Color.FromHex("#35c659"),
                ExcuseState.Rejected => Color.FromHex("#dc4e41"),
                ExcuseState.Unreviewed => Color.CornflowerBlue,
                _ => Color.Cyan
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}