using CommonShared.DataModels;
using Xamarin.Forms.Internals;

namespace AppShared.Controls
{
    [Preserve(AllMembers = true)]
    public class SearchableExcusesList : SearchableListView
    {
        public override bool FilterContacts(object obj)
        {
            if (base.FilterContacts(obj))
            {
                if (obj is not Excuse excuse)
                {
                    return false;
                }

                return excuse.Course.Name.ToUpperInvariant().Contains(SearchText.ToUpperInvariant())
                       || excuse.Period.ToUpperInvariant().Contains(SearchText.ToUpperInvariant());
            }

            return false;
        }
    }
}