using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AppShared.Services;
using CommonShared.DataModels;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StudentEnd.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace StudentEnd.ViewModels
{
    public class AttendancesListPageViewModel : BaseViewModel
    {
        public RangeObservableCollection<Attendance> AttendancesList { get; set; } = new();
        public string SearchText { get; set; }

        public Command<ItemTappedEventArgs> ItemTappedCommand { get; set; }
        public Command ToggleRecordsCommand { get; set; }

        public AttendancesListPageViewModel()
        {
            ItemTappedCommand = new Command<ItemTappedEventArgs>(ProcessAttendance);
            ToggleRecordsCommand = new Command(ToggleRecordsClicked);
        }

        public async Task RefreshList()
        {
            var attendances = await HttpClient.GetFromJsonAsync<List<Attendance>>("student/attendance/GetAttendances");
            if (attendances is not null)
            {
                AttendancesList.ReplaceRange(attendances);
            }
        }

        private async void ProcessAttendance(ItemTappedEventArgs eventArgs)
        {
            if (eventArgs.ItemData is not Attendance attendance)
            {
                return;
            }

            var attendanceResult = AttendanceResult.Normal;

            if (attendance.EndTime < DateTime.Now)
            {
                attendanceResult = AttendanceResult.Late;
            }

            if (await MessageService.DisplayConfirm("是否进行签到"))
            {
                if (attendance.Type is AttendanceType.Location or AttendanceType.Both)
                {
                    var attendanceLocation =
                        JsonSerializer.Deserialize<Dictionary<string, double>>(attendance.Location);
                    var cts = new CancellationTokenSource();
                    var geoRequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                    var currentLocation = await Geolocation.GetLocationAsync(geoRequest, cts.Token);
                    if (attendanceLocation != null)
                    {
                        var distance = Location.CalculateDistance(
                            new Location(attendanceLocation["Latitude"],
                                attendanceLocation["Longitude"]), currentLocation, DistanceUnits.Kilometers) * 1000;
                        if (distance > 200)
                        {
                            MessageService.Alert("距离签到位置过远，签到失败");
                            return;
                        }
                    }
                }

                if (attendance.Type is AttendanceType.Face or AttendanceType.Both)
                {
                    var imageResult = await MediaPicker.CapturePhotoAsync();
                    if (imageResult is null)
                    {
                        return;
                    }

                    var faceImageStream = await imageResult.OpenReadAsync();

                    var faceImageData = new byte[faceImageStream.Length];
                    await faceImageStream.ReadAsync(faceImageData, 0, faceImageData.Length);
                    faceImageStream.Close();
                    var faceData = Convert.ToBase64String(faceImageData);
                    if (App.StudentFace is null or "")
                    {
                        await HttpClient.PostAsJsonAsync("student/SaveFaceData", faceData);
                    }
                    else
                    {
                        var faceService = App.ServiceProvider.GetRequiredService<FaceRecognitionService>();
                        var jsonString = faceService.GetFaceMatchJsonResult(App.StudentFace, faceData);
                        var document = JsonDocument.Parse(jsonString);
                        var errorCode = document.RootElement.EnumerateObject()
                            .First(property => property.NameEquals("error_code"));
                        if (errorCode.Value.GetInt32() == 0)
                        {
                            var result = document.RootElement.EnumerateObject()
                                .First(property => property.NameEquals("result"));
                            var score = result.Value.EnumerateObject().First(property => property.NameEquals("score"));
                            if (score.Value.GetDouble() < 80)
                            {
                                MessageService.Alert("人脸验证失败，请重新尝试签到");
                                return;
                            }
                        }
                        else
                        {
                            MessageService.Alert("人脸验证失败，请重新尝试签到");
                            return;
                        }
                    }
                }

                var response = await HttpClient.PostAsJsonAsync("student/attendance/ProcessAttendance",
                    new AttendanceRecord
                    {
                        AttendanceId = attendance.Id,
                        Result = attendanceResult,
                    });
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    MessageService.Alert("签到成功");
                    IsBusy = true;
                    await RefreshList();
                    IsBusy = false;
                    return;
                }

                MessageService.Alert("签到失败");
            }
        }

        private async void ToggleRecordsClicked()
        {
            IsBusy = true;
            await Shell.Current.GoToAsync(nameof(AttendanceRecordsListPage));
            IsBusy = false;
        }
    }
}