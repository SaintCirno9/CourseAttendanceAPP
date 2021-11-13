using CommonShared.DataModels;

namespace AppShared.Controls
{
    public class StudentSearchableAttendanceRecordsList : SearchableListView
    {
        public override bool FilterContacts(object obj)
        {
            if (!base.FilterContacts(obj))
            {
                return false;
            }

            if (obj is not AttendanceRecord attendanceRecord)
            {
                return false;
            }

            return attendanceRecord.Attendance.Course.Name.ToUpperInvariant()
                       .Contains(SearchText.ToUpperInvariant())
                   || attendanceRecord.Attendance.Course.Number.ToUpperInvariant()
                       .Contains(SearchText.ToUpperInvariant());
        }
    }
}