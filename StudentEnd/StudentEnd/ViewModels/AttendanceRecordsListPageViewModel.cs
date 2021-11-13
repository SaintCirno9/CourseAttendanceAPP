using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommonShared.DataModels;
using StudentEnd.Views;
using Xamarin.Forms;

namespace StudentEnd.ViewModels
{
    public class AttendanceRecordsListPageViewModel : BaseViewModel
    {
        public RangeObservableCollection<AttendanceRecord> AttendanceRecordsList { get; set; } = new();
        public string SearchText { get; set; }

        public Command ToggleAttendancesCommand { get; set; }
        public Command<ItemTappedEventArgs> ItemTappedCommand { get; set; }

        public AttendanceRecordsListPageViewModel()
        {
            ToggleAttendancesCommand = new Command(ToggleAttendancesClicked);
        }

        public async Task RefreshList()
        {
            var attendanceRecords =
                await HttpClient.GetFromJsonAsync<List<AttendanceRecord>>("student/attendance/GetAttendanceRecords");
            AttendanceRecordsList.ReplaceRange(attendanceRecords ?? new List<AttendanceRecord>());
        }

        private async void ToggleAttendancesClicked()
        {
            IsBusy = true;
            await Shell.Current.GoToAsync(nameof(AttendancesListPage));
            IsBusy = false;
        }
    }
}