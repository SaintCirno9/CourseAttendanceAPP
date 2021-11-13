using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CommonShared.DataModels;
using Xamarin.Forms;

namespace AppShared.Converters
{
    public class AttendanceResultColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                AttendanceRecord attendanceRecord => attendanceRecord.Result switch
                {
                    AttendanceResult.Absent => Color.FromHex("#dc4e41"),
                    AttendanceResult.LeaveEarly or AttendanceResult.Late => Color.FromHex("#fcbc0f"),
                    AttendanceResult.Normal or AttendanceResult.Excuse => Color.FromHex("#35c659"),
                    _ => Color.FromHex("#dc4e41")
                },
                _ => Color.Black
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}