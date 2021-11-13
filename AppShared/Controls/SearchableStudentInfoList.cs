using CommonShared.DataModels;

namespace AppShared.Controls
{
    public class SearchableStudentInfoList: SearchableListView
    {
        public override bool FilterContacts(object obj)
        {
            if (base.FilterContacts(obj))
            {
                if (obj is not Student student)
                {
                    return false;
                }

                return student.Name.ToUpperInvariant().Contains(SearchText.ToUpperInvariant())
                       || student.Id.ToUpperInvariant().Contains(SearchText.ToUpperInvariant());
            }

            return false;
        }
    }
}