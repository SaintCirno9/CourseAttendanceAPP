using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommonShared.DataModels;
using StudentEnd.Views;
using Xamarin.Forms;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace StudentEnd.ViewModels
{
    public class ExcusesListPageViewModel : BaseViewModel
    {
        public RangeObservableCollection<Excuse> ExcusesList { get; set; } = new();
        public string SearchText { get; set; }

        public Command AddCommand { get; set; }
        public Command<ItemTappedEventArgs> ItemTappedCommand { get; set; }

        public ExcusesListPageViewModel()
        {
            AddCommand = new Command(NavigateToAddExcusePage);
            ItemTappedCommand = new Command<ItemTappedEventArgs>(NavigateToExcuseDetailPage);
        }

        public async Task RefreshList()
        {
            var excuses = await HttpClient.GetFromJsonAsync<List<Excuse>>("student/excuse/GetExcuses");
            if (excuses is not null)
            {
                ExcusesList.ReplaceRange(excuses);
            }
        }

        private void NavigateToAddExcusePage()
        {
            ExcuseDetailPageViewModel.Excuse = null;
            Shell.Current.GoToAsync(nameof(ExcuseDetailPage));
        }

        private void NavigateToExcuseDetailPage(ItemTappedEventArgs eventArgs)
        {
            ExcuseDetailPageViewModel.Excuse = eventArgs.ItemData as Excuse;
            Shell.Current.GoToAsync(nameof(ExcuseDetailPage));
        }
    }
}