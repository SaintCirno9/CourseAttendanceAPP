using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using CommonShared.DataModels;
using Microsoft.Extensions.DependencyInjection;
using StudentEnd.ViewModels;
using StudentEnd.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace StudentEnd.ViewModels
{
    /// <summary>
    /// ViewModel for courses page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class CoursesListPageViewModel : BaseViewModel
    {
        public static bool ViewAttended { get; set; }
        public string PageTitle { get; set; } = "可选课程";
        public string SearchText { get; set; }


        public RangeObservableCollection<Course> CoursesList { get; set; } = new();


        public Command<ItemTappedEventArgs> ItemTappedCommand { get; set; }
        public Command ToggleCommand { get; set; }

        public CoursesListPageViewModel()
        {
            ItemTappedCommand = new Command<ItemTappedEventArgs>(NavigateToCourseDetailPage);
            ToggleCommand = new Command(ToggleAttendedClicked);
        }

        public async Task RefreshList()
        {
            var courses = ViewAttended
                ? await HttpClient
                    .GetFromJsonAsync<List<Course>>("student/course/GetAttendedCourses")
                : await HttpClient
                    .GetFromJsonAsync<List<Course>>("student/course/GetCourses");
            if (courses is null)
            {
                return;
            }

            CoursesList.ReplaceRange(courses);
        }


        private async void NavigateToCourseDetailPage(ItemTappedEventArgs eventArgs)
        {
            IsBusy = true;
            CourseDetailPageViewModel.Course = eventArgs.ItemData as Course;
            await Shell.Current.GoToAsync($"{nameof(CourseDetailPage)}?attended={ViewAttended}");
            IsBusy = false;
        }

        private async void ToggleAttendedClicked()
        {
            IsBusy = true;
            ViewAttended = !ViewAttended;
            PageTitle = PageTitle == "可选课程" ? "已选课程" : "可选课程";
            OnPropertyChanged(nameof(PageTitle));
            await RefreshList();
            IsBusy = false;
        }
    }
}