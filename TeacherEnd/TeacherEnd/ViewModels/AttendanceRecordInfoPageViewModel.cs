using System.Collections.ObjectModel;
using CommonShared.DataModels;
using TeacherEnd.Views;
using Xamarin.Forms;

namespace TeacherEnd.ViewModels
{
    public class AttendanceRecordInfoPageViewModel : BaseViewModel
    {
        public static Attendance Attendance { get; set; }
        public RangeObservableCollection<AttendanceRecord> AttendanceRecordList { get; set; } = new();
        public string SearchText { get; set; }
        public Command<ItemTappedEventArgs> ItemTappedCommand { get; set; }
        public Command BackCommand { get; set; }
        public Command GoToDetailCommand { get; set; }

        public AttendanceRecordInfoPageViewModel()
        {
            BackCommand = new Command(BackClicked);
            GoToDetailCommand = new Command(GoToDetailClicked);
        }

        public void Init()
        {
            if (Attendance is null)
            {
                return;
            }

            AttendanceRecordList.ReplaceRange(Attendance.AttendanceRecords);
        }

        private async void BackClicked()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void GoToDetailClicked()
        {
            AttendanceDetailPageViewModel.Attendance = Attendance;
            await Shell.Current.GoToAsync(nameof(AttendanceDetailPage));
        }
    }
}