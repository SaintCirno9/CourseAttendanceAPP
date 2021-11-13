using CommonShared.DataModels;

namespace AppShared.Controls
{
    public class TeacherSearchableAttendanceRecordsList: SearchableListView
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

            return attendanceRecord.Student.Name.ToUpperInvariant()
                       .Contains(SearchText.ToUpperInvariant())
                   || attendanceRecord.Student.Id.ToUpperInvariant()
                       .Contains(SearchText.ToUpperInvariant());
        }
    }
}