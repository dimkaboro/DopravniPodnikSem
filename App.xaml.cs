using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using DopravniPodnikSem.Services;
using Microsoft.EntityFrameworkCore;


namespace DopravniPodnikSem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration Configuration { get; }
        public ServiceProvider ServiceProvider { get; }

        public App()
        {
            // Настройка конфигурации
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // Настройка сервисов
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Регистрация IConfiguration
            services.AddSingleton<IConfiguration>(Configuration);

            // Регистрация DbContext с использованием строки подключения из конфигурации
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseOracle(Configuration.GetConnectionString("DefaultConnection")));

            // Регистрация сервисов
            services.AddTransient<DatabaseService>();
            // Добавьте другие сервисы по мере необходимости

            // Регистрация Views
            services.AddSingleton<MainWindow>();
            // Добавьте другие Views по мере необходимости
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Получение MainWindow из DI контейнера
            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}



