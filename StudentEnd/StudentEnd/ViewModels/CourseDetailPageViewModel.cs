using System.Collections.Generic;
using System.Net;
using System.Web;
using CommonShared.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StudentEnd.ViewModels
{
    /// <summary>
    /// ViewModel for Business Registration Form page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class CourseDetailPageViewModel : DetailPageViewModel, IQueryAttributable
    {
        public bool IsAttendedCourse { get; set; }
        public static Course Course { get; set; }

        public string CourseName { get; set; }
        public string CourseNumber { get; set; }
        public int CourseSerial { get; set; }
        public int CourseDuration { get; set; }
        public Command SelectCommand { get; set; }
        public Command ExitCommand { get; set; }

        public CourseDetailPageViewModel()
        {
            PageTitle = "课程信息";
            BackCommand = new Command(BackClicked);
            SelectCommand = new Command(SelectClicked);
            ExitCommand = new Command(ExitClicked);
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            IsAttendedCourse = HttpUtility.UrlDecode(query["attended"]) == "True";
            OnPropertyChanged(nameof(IsAttendedCourse));
        }

        public void Init()
        {
            if (Course is null)
            {
                return;
            }
            CourseName = Course.Name;
            CourseNumber = Course.Number;
            CourseSerial = Course.Serial;
            CourseDuration = Course.Duration;
            OnPropertyChanged(nameof(CourseName));
            OnPropertyChanged(nameof(CourseNumber));
            OnPropertyChanged(nameof(CourseSerial));
            OnPropertyChanged(nameof(CourseDuration));
        }


        private async void SelectClicked(object obj)
        {
            var response = await HttpClient.GetAsync($"student/course/ChooseCourses/{Course.Id}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageService.Alert("选课成功");
                BackClicked();
            }
        }

        private async void ExitClicked(object obj)
        {
            var response = await HttpClient.GetAsync($"student/course/ExitCourses/{Course.Id}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageService.Alert("退课成功");
                BackClicked();
            }
        }
    }
}