using CommonShared.DataModels;

namespace AppShared.Controls
{
    public class SearchableAttendancesList : SearchableListView
    {
        public override bool FilterContacts(object obj)
        {
            if (base.FilterContacts(obj))
            {
                if (obj is not Attendance attendance)
                {
                    return false;
                }

                return attendance.Course.Name.ToUpperInvariant().Contains(SearchText.ToUpperInvariant())
                       || attendance.Course.Number.ToUpperInvariant().Contains(SearchText.ToUpperInvariant());
            }

            return false;
        }
    }
}