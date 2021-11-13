using System;
using System.Globalization;
using CommonShared.DataModels;
using Xamarin.Forms;

namespace AppShared.Converters
{
    public class CourseInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Course course)
            {
                return null;
            }

            return
                $"{course.Name} ({course.Number}-{course.Serial switch {<10 => $"0{course.Serial}", _ => course.Serial}})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}