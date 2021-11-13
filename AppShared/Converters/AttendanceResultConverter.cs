using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CommonShared.DataModels;
using Xamarin.Forms;

namespace AppShared.Converters
{
    public class AttendanceResultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                AttendanceRecord attendanceRecord => attendanceRecord.Result switch
                {
                    AttendanceResult.Absent => "缺勤",
                    AttendanceResult.LeaveEarly => "早退",
                    AttendanceResult.Late => "迟到",
                    AttendanceResult.Normal => "已签到",
                    AttendanceResult.Excuse => "已请假",
                    _ => "未签到"
                },
                _ => "空"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}