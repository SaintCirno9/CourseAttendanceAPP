using System;
using Syncfusion.Licensing;
using TeacherEnd.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherEnd
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; set; }


        public App()
        {
            SyncfusionLicenseProvider.RegisterLicense(
                "NDMyMTY4QDMxMzkyZTMxMmUzMG80cjFwUUtHaEpNbXJ6QmE4S1lxNmpEdzZ3NEtSbktDZDRvZUQzdzZwcFk9");
            InitializeComponent();

            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}