using Xamarin.Forms;

namespace TeacherEnd.ViewModels
{
    public class DetailPageViewModel: BaseViewModel
    {
        public string PageTitle { get; set; }
        public Command BackCommand { get; set; }

        protected DetailPageViewModel()
        {
            BackCommand = new Command(BackClicked);
        }
        

        protected async void BackClicked()
        {
            await Shell.Current.GoToAsync("..", true);
        }
    }
}