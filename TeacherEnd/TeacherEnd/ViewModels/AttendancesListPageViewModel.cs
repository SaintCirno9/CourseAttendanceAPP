using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommonShared.DataModels;
using Microsoft.Extensions.DependencyInjection;
using TeacherEnd.Views;
using Xamarin.Forms;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace TeacherEnd.ViewModels
{
    public class AttendancesListPageViewModel : BaseViewModel
    {
        public RangeObservableCollection<Attendance> AttendanceList { get; set; } = new();
        public string SearchText { get; set; }

        public Command<ItemTappedEventArgs> ItemTappedCommand { get; set; }
        public Command AddCommand { get; set; }

        public AttendancesListPageViewModel()
        {
            ItemTappedCommand = new Command<ItemTappedEventArgs>(NavigateToAttendanceDetailPage);
            AddCommand = new Command(NavigateToAddAttendancePage);
        }

        public async Task RefreshList()
        {
            var attendances =
                await HttpClient.GetFromJsonAsync<List<Attendance>>("teacher/attendance/GetAttendances");
            if (attendances is not null)
            {
                AttendanceList.ReplaceRange(attendances);
            }
        }

        private void NavigateToAttendanceDetailPage(ItemTappedEventArgs eventArgs)
        {
            var attendance = eventArgs.ItemData as Attendance;
            AttendanceRecordInfoPageViewModel.Attendance = attendance;
            Shell.Current.GoToAsync(nameof(AttendanceRecordInfoPage));
        }

        private void NavigateToAddAttendancePage()
        {
            AttendanceDetailPageViewModel.Attendance = null;
            Shell.Current.GoToAsync("AttendanceDetailPage");
        }
    }
}