using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TeacherEnd.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(Startup.Init(ConfigureServices));
        }

        void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
        }
    }
}