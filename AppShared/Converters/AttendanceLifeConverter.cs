using System;
using System.Globalization;
using CommonShared.DataModels;
using Xamarin.Forms;

namespace AppShared.Converters
{
    public class AttendanceLifeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Attendance attendance)
            {
                return null;
            }

            var state = "";
            var now = DateTime.Now;
            if (now > attendance.DeadTime)
            {
                state = "已截止";
            }
            else if (now > attendance.EndTime)
            {
                state = "可签到";
            }
            else if (now >= attendance.StartTime)
            {
                state = "正在进行";
            }
            else
            {
                state = "数据错误";
            }

            return state;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}