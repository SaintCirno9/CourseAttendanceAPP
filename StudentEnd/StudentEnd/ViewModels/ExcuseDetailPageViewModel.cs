using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AppShared.Validators;
using AppShared.Validators.Rules;
using CommonShared.DataModels;
using StudentEnd.Views;
using Xamarin.Forms;

namespace StudentEnd.ViewModels
{
    public class ExcuseDetailPageViewModel : DetailPageViewModel
    {
        public bool CanSave { get; set; }
        public bool CanDelete { get; set; }

        public bool CanEdit { get; set; }
        public static Excuse Excuse { get; set; }
        public RangeObservableCollection<string> AttendedCourseNames { get; set; } = new();
        private RangeObservableCollection<Course> AttendedCourses { get; set; } = new();

        public ValidatableObject<string> ExcuseReason { get; set; } = new();
        public ValidatableObject<string> ExcusePeriod { get; set; } = new();

        public string CourseName { get; set; }

        public Command DeleteCommand { get; set; }
        public Command SaveCommand { get; set; }

        public ExcuseDetailPageViewModel()
        {
            PageTitle = "请假信息";
            BackCommand = new Command(BackClicked);
            SaveCommand = new Command(SaveClicked);
            DeleteCommand = new Command(DeleteClicked);
        }

        public async Task Init()
        {
            ExcuseReason = new ValidatableObject<string>();
            ExcusePeriod = new ValidatableObject<string>();
            ExcusePeriod.ValidationRules.Add(new IsNotNullOrEmptyRule<string>()
                {ValidationMessage = "Period Required"});
            ExcuseReason.ValidationRules.Add(new IsNotNullOrEmptyRule<string>()
                {ValidationMessage = "Reason Required"});

            if (Excuse is null)
            {
                CanSave = true;
                CanDelete = false;
                CanEdit = true;
            }
            else
            {
                CanSave = Excuse.ExcuseState == ExcuseState.Unreviewed;
                CanDelete = Excuse.ExcuseState == ExcuseState.Unreviewed;
                CanEdit = Excuse.ExcuseState == ExcuseState.Unreviewed;
                ExcusePeriod.Value = Excuse.Period;
                ExcuseReason.Value = Excuse.Reason;
            }

            OnPropertyChanged(nameof(CanSave));
            OnPropertyChanged(nameof(CanDelete));
            OnPropertyChanged(nameof(CanEdit));

            OnPropertyChanged(nameof(ExcusePeriod));
            OnPropertyChanged(nameof(ExcuseReason));

            await GetAttendedCourses();
        }

        private async Task GetAttendedCourses()
        {
            var attendedCourses = await HttpClient.GetFromJsonAsync<List<Course>>("student/course/GetAttendedCourses");
            if (attendedCourses is not null)
            {
                AttendedCourses.ReplaceRange(attendedCourses);
                AttendedCourseNames.ReplaceRange(attendedCourses.Select(course => course.Name));
                if (Excuse is null)
                {
                    CourseName = AttendedCourseNames?.FirstOrDefault();
                }
                else
                {
                    CourseName = Excuse.Course.Name;
                }
            }
            else
            {
                MessageService.Alert("请先选课！");
                BackClicked();
                return;
            }


            OnPropertyChanged(nameof(CourseName));
        }

        private async void SaveClicked()
        {
            ExcusePeriod.Validate();
            ExcuseReason.Validate();
            if (!ExcusePeriod.IsValid || !ExcuseReason.IsValid)
            {
                return;
            }

            HttpResponseMessage response;
            if (Excuse == null)
            {
                response = HttpClient.PostAsJsonAsync("student/excuse/SaveExcuse", new Excuse
                {
                    Reason = ExcuseReason.Value,
                    Period = ExcusePeriod.Value,
                    CourseId = AttendedCourses.First(course => course.Name == CourseName).Id,
                }).Result;
            }
            else
            {
                Excuse.Period = ExcusePeriod.Value;
                Excuse.Reason = ExcuseReason.Value;
                Excuse.CourseId = AttendedCourses.First(course => course.Name == CourseName).Id;
                response = await HttpClient.PostAsJsonAsync("student/excuse/SaveExcuse", Excuse);
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageService.Alert("保存成功");
                BackClicked();
                return;
            }

            MessageService.Alert("保存失败");
        }

        private async void DeleteClicked()
        {
            var response = await HttpClient.DeleteAsync($"student/excuse/DeleteExcuse/{Excuse.Id}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageService.Alert("删除成功");
                BackClicked();
                return;
            }

            MessageService.Alert("删除失败");
        }
    }
}