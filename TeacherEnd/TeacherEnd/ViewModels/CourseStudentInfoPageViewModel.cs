using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommonShared.DataModels;
using TeacherEnd.Views;
using Xamarin.Forms;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace TeacherEnd.ViewModels
{
    public class CourseStudentInfoPageViewModel : BaseViewModel
    {
        public static Course Course { get; set; }
        public string SearchText { get; set; }
        public RangeObservableCollection<Student> StudentList { get; set; } = new();

        public Command<ItemTappedEventArgs> ItemTappedCommand { get; set; }
        public Command BackCommand { get; set; }
        public Command GoToDetailCommand { get; set; }

        public CourseStudentInfoPageViewModel()
        {
            BackCommand = new Command(BackClicked);
            GoToDetailCommand = new Command(GoToDetailClicked);
        }

        public void Init()
        {
            if (Course is null)
            {
                return;
            }

            StudentList.ReplaceRange(Course.Students);
        }

        private async void BackClicked()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void GoToDetailClicked()
        {
            CourseDetailPageViewModel.Course = Course;
            await Shell.Current.GoToAsync(nameof(CourseDetailPage));
        }
    }
}