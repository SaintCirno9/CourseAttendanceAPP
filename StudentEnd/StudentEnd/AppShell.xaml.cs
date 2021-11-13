using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentEnd.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudentEnd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CoursesListPage), typeof(CoursesListPage));
            Routing.RegisterRoute(nameof(ExcusesListPage), typeof(ExcusesListPage));
            Routing.RegisterRoute(nameof(AttendancesListPage), typeof(AttendancesListPage));
            Routing.RegisterRoute(nameof(AttendanceRecordsListPage), typeof(AttendanceRecordsListPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(CourseDetailPage), typeof(CourseDetailPage));
            Routing.RegisterRoute(nameof(ExcuseDetailPage), typeof(ExcuseDetailPage));
            Routing.RegisterRoute(nameof(AccountInfoPage), typeof(AccountInfoPage));
            Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
            Routing.RegisterRoute(nameof(ChangeEmailPage), typeof(ChangeEmailPage));
            Routing.RegisterRoute(nameof(ChangePhonePage), typeof(ChangePhonePage));
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            // Don't clear navigation stacks while we're already in the process of clearing stacks.
            if (args.Source == ShellNavigationSource.PopToRoot)
            {
                return;
            }

            /*
             Clear all navigation stacks before navigating to our destination, that way we don't
            have to avoid the destination stack because it does not yet contain pages to clear.
            */
            foreach (var item in TabBar.Items)
            {
                if (item?.Stack?.Count > 1)
                {
                    // 0 is null in my case so I just use the first page in the stack that is usable, you might have to adjust that in your own code
                    item.Stack[1].Navigation.PopToRootAsync();
                }
            }
        }
    }
}