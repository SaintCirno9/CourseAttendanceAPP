using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using AppShared.Validators;
using AppShared.Validators.Rules;
using CommonShared.DataModels;
using Microsoft.Extensions.DependencyInjection;
using TeacherEnd.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TeacherEnd.ViewModels
{
    [Preserve(AllMembers = true)]
    public class CourseDetailPageViewModel : DetailPageViewModel
    {
        public bool IsModifying { get; set; }
        public static Course Course { get; set; }
        public ValidatableObject<string> CourseName { get; set; }
        public ValidatableObject<string> CourseNumber { get; set; }

        public ValidatableObject<int> CourseSerial { get; set; }

        public ValidatableObject<int> CourseDuration { get; set; }


        public Command SaveCommand { get; set; }

        public Command DeleteCommand { get; set; }

        public CourseDetailPageViewModel()
        {
            PageTitle = "课程信息";
            

            SaveCommand = new Command(SaveClicked);
            DeleteCommand = new Command(DeleteClicked);

            
        }

        public void Init()
        {
            CourseName = new ValidatableObject<string>();
            CourseNumber = new ValidatableObject<string>();
            CourseSerial = new ValidatableObject<int>();
            CourseDuration = new ValidatableObject<int>();
            CourseName.ValidationRules.Add(
                new IsNotNullOrEmptyRule<string> {ValidationMessage = "Course Name Required"});
            CourseNumber.ValidationRules.Add(new IsNotNullOrEmptyRule<string>
                {ValidationMessage = "Course Number Required"});
            CourseSerial.ValidationRules.Add(new IsNotZeroRule<int>
                {ValidationMessage = "Course Serial Should Not Be 0"});
            CourseDuration.ValidationRules.Add(new IsNotZeroRule<int>
                {ValidationMessage = "Course Duration Should Not Be 0"});
            if (Course is not null)
            {
                CourseName.Value = Course.Name;
                CourseNumber.Value = Course.Number;
                CourseSerial.Value = Course.Serial;
                CourseDuration.Value = Course.Duration;
                IsModifying = true;
            }
            else
            {
                IsModifying = false;
            }

            OnPropertyChanged(nameof(IsModifying));
            OnPropertyChanged(nameof(CourseName));
            OnPropertyChanged(nameof(CourseNumber));
            OnPropertyChanged(nameof(CourseSerial));
            OnPropertyChanged(nameof(CourseDuration));
        }

        private async void SaveClicked(object obj)
        {
            CourseName.Validate();
            CourseNumber.Validate();
            CourseSerial.Validate();
            CourseDuration.Validate();
            if (CourseName.IsValid && CourseNumber.IsValid && CourseSerial.IsValid && CourseDuration.IsValid)
            {
                HttpResponseMessage response;
                if (Course is null)
                {
                    response = await HttpClient.PostAsJsonAsync("teacher/course/saveCourse", new Course
                    {
                        Name = CourseName.Value,
                        Number = CourseNumber.Value,
                        Serial = CourseSerial.Value,
                        Duration = CourseDuration.Value
                    });
                }
                else
                {
                    Course.Name = CourseName.Value;
                    Course.Number = CourseNumber.Value;
                    Course.Serial = CourseSerial.Value;
                    Course.Duration = CourseDuration.Value;
                    response = await HttpClient.PostAsJsonAsync("teacher/course/saveCourse", Course);
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    MessageService.Alert("添加成功");
                    BackClicked();
                    return;
                }

                MessageService.Alert("添加失败");
            }
        }

        private async void DeleteClicked(object obj)
        {
            var response = await HttpClient.DeleteAsync($"teacher/course/deleteCourse/{Course.Id}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageService.Alert("删除成功");
                BackClicked();
                return;
            }

            MessageService.Alert("删除失败");
            BackClicked();
        }
    }
}