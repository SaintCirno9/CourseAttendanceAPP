using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AppShared.Validators;
using AppShared.Validators.Rules;
using CommonShared.DataModels;
using Microsoft.Extensions.DependencyInjection;
using TeacherEnd.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TeacherEnd.ViewModels
{
    public class AttendanceDetailPageViewModel : DetailPageViewModel
    {
        public bool IsAdding { get; set; }
        public static Attendance Attendance { get; set; }
        public string AttendanceTypeString { get; set; }
        public List<string> AttendanceTypeStrings { get; set; }

        private RangeObservableCollection<Course> Courses { get; set; } = new();

        public string CourseName { get; set; }
        public RangeObservableCollection<string> CourseNames { get; set; } = new();
        public ValidatableObject<int> Duration { get; set; }
        public ValidatableObject<int> MaxDuration { get; set; }
        public Command SaveCommand { get; set; }
        public Command DeleteCommand { get; set; }

        public AttendanceDetailPageViewModel()
        {
            PageTitle = "签到信息";

            SaveCommand = new Command(SaveClicked);
            DeleteCommand = new Command(DeleteClicked);
            AttendanceTypeStrings = new List<string>
            {
                GetAttendanceTypeString(AttendanceType.Location),
                GetAttendanceTypeString(AttendanceType.Face),
                GetAttendanceTypeString(AttendanceType.Both)
            };
        }

        public async Task Init()
        {
            Duration = new ValidatableObject<int>();
            MaxDuration = new ValidatableObject<int>();
            Duration.ValidationRules.Add(new IsNotZeroRule<int>() {ValidationMessage = "持续时长不能为0或空"});
            MaxDuration.ValidationRules.Add(new IsNotZeroRule<int>() {ValidationMessage = "有效时长不能为0或空"});

            await GetCourses();
            if (Attendance is not null)
            {
                AttendanceTypeString = GetAttendanceTypeString(Attendance.Type);
                CourseName = ConvertCourseName(Attendance.Course);
                Duration.Value = (int) Attendance.EndTime.Subtract(Attendance.StartTime).TotalMinutes;
                MaxDuration.Value = (int) Attendance.DeadTime.Subtract(Attendance.StartTime).TotalMinutes;
                IsAdding = false;
            }
            else
            {
                IsAdding = true;
                AttendanceTypeString = AttendanceTypeStrings[0];
            }

            OnPropertyChanged(nameof(IsAdding));
            OnPropertyChanged(nameof(Duration));
            OnPropertyChanged(nameof(MaxDuration));
            OnPropertyChanged(nameof(CourseName));
            OnPropertyChanged(nameof(AttendanceTypeString));
        }

        private async Task GetCourses()
        {
            var courses = await HttpClient.GetFromJsonAsync<List<Course>>("teacher/course/GetCourses");
            if (courses is null)
            {
                return;
            }

            Courses.ReplaceRange(courses);
            CourseNames.ReplaceRange(Courses.Select(ConvertCourseName));

            if (Attendance is null)
            {
                CourseName = CourseNames?[0];
                OnPropertyChanged(nameof(CourseName));
            }
        }

        private string ConvertCourseName(Course course) =>
            course.Name + "-" + course.Serial switch {<10 => $"0{course.Serial}", _ => course.Serial};


        private string GetAttendanceTypeString(AttendanceType type)
        {
            return type switch
            {
                AttendanceType.Location => "地理定位",
                AttendanceType.Face => "人脸识别",
                AttendanceType.Both => "双重验证",
                _ => "未知"
            };
        }

        private async void SaveClicked()
        {
            var startTime = DateTime.Now;
            var endTime = startTime.AddMinutes(Duration.Value);
            var deadTime = startTime.AddMinutes(MaxDuration.Value);
            var location = "";
            var courseId = Courses.First(course => ConvertCourseName(course) == CourseName).Id;
            var type = AttendanceTypeString switch
            {
                "地理定位" => AttendanceType.Location,
                "人脸识别" => AttendanceType.Face,
                "双重验证" => AttendanceType.Both,
                _ => AttendanceType.Location
            };
            if (type is AttendanceType.Location or AttendanceType.Both)
            {
                var geoRequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var cts = new CancellationTokenSource();
                var geoLocation = await Geolocation.GetLocationAsync(geoRequest, cts.Token);
                if (geoLocation is not null)
                {
                    location = JsonSerializer.Serialize(new
                    {
                        geoLocation.Longitude, geoLocation.Latitude
                    });
                }
                else
                {
                    MessageService.Alert("无法获取位置信息，请检查设备");
                    return;
                }
            }

            var response = await HttpClient.PostAsJsonAsync("teacher/attendance/SaveAttendance", new Attendance
            {
                StartTime = startTime,
                EndTime = endTime,
                DeadTime = deadTime,
                Location = location,
                Type = type,
                CourseId = courseId
            });

            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageService.Alert("保存成功");
                BackClicked();
                return;
            }

            MessageService.Alert("保存失败" + response.StatusCode);
        }

        private async void DeleteClicked()
        {
            var response = HttpClient.DeleteAsync($"teacher/attendance/DeleteAttendance/{Attendance.Id}").Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageService.Alert("删除成功");
                await Shell.Current.GoToAsync(nameof(AttendancesListPage));
                return;
            }

            MessageService.Alert("删除失败");
        }
    }
}