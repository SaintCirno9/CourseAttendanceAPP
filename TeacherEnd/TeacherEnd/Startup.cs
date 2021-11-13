using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using AppShared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Xamarin.Essentials;

namespace TeacherEnd
{
    public static class Startup
    {
        public static App Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices)
        {
            var systemDir = FileSystem.CacheDirectory;
            ExtractSaveResource("TeacherEnd.appsettings.json", systemDir);
            var fullConfig = Path.Combine(systemDir, "TeacherEnd.appsettings.json");

            var host = new HostBuilder()
                .ConfigureHostConfiguration(builder =>
                {
                    builder.AddCommandLine(new[] {$"ContentRoot={FileSystem.AppDataDirectory}"});
                    builder.AddJsonFile(fullConfig);
                })
                .ConfigureServices((context, services) =>
                {
                    nativeConfigureServices(context, services);
                    ConfigureServices(context, services);
                })
                .ConfigureLogging(builder => builder.AddSimpleConsole(options =>
                {
                    options.ColorBehavior = LoggerColorBehavior.Disabled;
                }))
                .Build();

            App.ServiceProvider = host.Services;
            return App.ServiceProvider.GetService<App>();
        }


        static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };
            var httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(context.Configuration["BaseAddress"]),
                Timeout = TimeSpan.FromSeconds(30)
            };

            services.AddSingleton(httpClient);

            services.AddSingleton<App>();
            services.AddSingleton<MessageService>();
            services.AddSingleton(
                new LoginDatabaseService(Path.Combine(FileSystem.AppDataDirectory, "TeacherEnd.db3")));

            // Modify Default JsonSerializeOptions used in System.Net.Http.Json (HttpClient Extended Methods)
            if (typeof(JsonContent)
                .GetField("s_defaultSerializerOptions", BindingFlags.Static | BindingFlags.NonPublic)
                ?.GetValue(null) is JsonSerializerOptions defaultJsonOptions)
            {
                defaultJsonOptions.ReferenceHandler = ReferenceHandler.Preserve;
                defaultJsonOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            }

            if (typeof(JsonSerializerOptions).GetField("s_defaultOptions", BindingFlags.Static | BindingFlags.NonPublic)
                ?.GetValue(null) is JsonSerializerOptions defaultJsonSerializerOptions)
            {
                defaultJsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                defaultJsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            }
        }

        private static void ExtractSaveResource(string filename, string location)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var resFilestream = assembly.GetManifestResourceStream(filename);
            if (resFilestream != null)
            {
                var full = Path.Combine(location, filename);

                using var stream = File.Create(full);
                resFilestream.CopyTo(stream);
            }
        }
    }
}