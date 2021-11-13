using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using CommonShared.DataModels;
using Microsoft.Extensions.DependencyInjection;
using TeacherEnd.ViewModels;
using TeacherEnd.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace TeacherEnd.ViewModels
{
    /// <summary>
    /// ViewModel for courses page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class CoursesListPageViewModel : BaseViewModel
    {
        public Command<ItemTappedEventArgs> ItemTappedCommand { get; set; }

        public Command AddCommand { get; set; }

        public RangeObservableCollection<Course> CoursesList { get; set; } = new();

        public string SearchText { get; set; }

        public CoursesListPageViewModel()
        {
            ItemTappedCommand = new Command<ItemTappedEventArgs>(GoToCourseStudentInfoPage);
            AddCommand = new Command(NavigateToAddCoursePage);
        }

        public async Task RefreshList()
        {
            var courses = await HttpClient.GetFromJsonAsync<List<Course>>("teacher/course/GetCourses");
            if (courses is not null)
            {
                CoursesList.ReplaceRange(courses);
            }
        }


        private void GoToCourseStudentInfoPage(ItemTappedEventArgs eventArgs)
        {
            var course = eventArgs.ItemData as Course;
            CourseStudentInfoPageViewModel.Course = course;
            Shell.Current.GoToAsync(nameof(CourseStudentInfoPage));
        }

        private void NavigateToAddCoursePage()
        {
            CourseDetailPageViewModel.Course = null;
            Shell.Current.GoToAsync(nameof(CourseDetailPage));
        }
    }
}