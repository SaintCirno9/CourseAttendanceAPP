using CommonShared.DataModels;
using Xamarin.Forms.Internals;

namespace AppShared.Controls
{
    /// <summary>
    /// This class extends the behavior of the SearchableListView control to filter the ListViewItem based on text.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SearchableCoursesList : SearchableListView
    {
        #region Method

        /// <summary>
        /// Filtering the list view items based on the search text.
        /// </summary>
        /// <param name="obj">The list view item</param>
        /// <returns>Returns the filtered item</returns>
        public override bool FilterContacts(object obj)
        {
            if (base.FilterContacts(obj))
            {
                if (obj is not Course course)
                {
                    return false;
                }

                return course.Name.ToUpperInvariant().Contains(SearchText.ToUpperInvariant())
                       || course.Number.ToUpperInvariant().Contains(SearchText.ToUpperInvariant());
            }

            return false;
        }

        #endregion
    }
}