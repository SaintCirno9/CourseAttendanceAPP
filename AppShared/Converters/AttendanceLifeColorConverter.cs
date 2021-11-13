using System;
using System.Globalization;
using CommonShared.DataModels;
using Xamarin.Forms;

namespace AppShared.Converters
{
    public class AttendanceLifeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Attendance attendance)
            {
                return null;
            }

            Color color;
            var now = DateTime.Now;
            if (now > attendance.DeadTime)
            {
                color = Color.FromHex("#dc4e41");
            }
            else if (now > attendance.EndTime)
            {
                color = Color.FromHex("#fcbc0f");
            }
            else if (now > attendance.StartTime)
            {
                color = Color.FromHex("#35c659");
            }
            else
            {
                color = Color.FromHex("#dc4e41");
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}