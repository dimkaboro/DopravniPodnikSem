using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using DopravniPodnikSem.Services;
using Microsoft.EntityFrameworkCore;
using DopravniPodnikSem.ViewModels;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Repository;



namespace DopravniPodnikSem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration Configuration { get; }
        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseOracle(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IUserDataRepository, UserDataRepository>();
            services.AddTransient<DatabaseService>();
            services.AddTransient<PasswordService>();

            services.AddSingleton<NavigationVM>();
            services.AddTransient<RegistrationViewModel>();
            services.AddTransient<LoginViewModel>();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = ServiceProvider.GetService<MainWindow>();
        }
    }
}



