using System;
using System.Globalization;
using System.Linq;
using CommonShared.DataModels;
using Xamarin.Forms;

namespace AppShared.Converters
{
    public class CourseStudentAttendanceColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Student student || parameter is not Course course)
            {
                return null;
            }

            var absentCount = course.Attendances.Count -
                              student.AttendanceRecords.Count(record =>
                                  record.Attendance.Course == course &&
                                  record.Result is AttendanceResult.Normal or AttendanceResult.Excuse);
            return absentCount == 0 ? Color.FromHex("#35c659") : Color.FromHex("#dc4e41");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}