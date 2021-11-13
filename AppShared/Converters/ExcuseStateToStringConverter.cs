using System;
using System.Globalization;
using CommonShared.DataModels;
using Xamarin.Forms;

namespace AppShared.Converters
{
    public class ExcuseStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ExcuseState excuseState)
            {
                return null;
            }

            return excuseState switch
            {
                ExcuseState.Passed => "通过",
                ExcuseState.Rejected => "驳回",
                ExcuseState.Unreviewed => "审核中",
                _ => "未知"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}