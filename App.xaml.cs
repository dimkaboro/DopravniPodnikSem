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
using DopravniPodnikSem.Views;



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
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<EditProfileViewModel>();
            services.AddTransient<UserEmulationViewModel>();
            //services.AddTransient<MainWindow>();
            //services.AddTransient<AdminWindow>();
            //services.AddTransient<EmployeeWindow>();


            services.AddTransient<IVozidloRepository, VozidloRepository>();
            services.AddTransient<VozidloViewModel>();

            services.AddTransient<IRidiciRepository, RidiciRepository>(); // Добавляем эту строку
            services.AddTransient<RidiciViewModel>(); // Уже есть

            services.AddTransient<ILinkyRepository, LinkyRepository>();
            services.AddTransient<LinkaViewModel>();

            services.AddTransient<IUdrzbaVozidlaRepository, UdrzbaVozidlaRepository>(); // Добавляем эту строку>
            services.AddTransient<UdrzbaVozidlaViewModel>();

            services.AddTransient<UdrzbaVozidlaViewModel>();
            services.AddTransient<SearchVozidloViewModel>();
            services.AddTransient<SearchVozidloWindow>();

            services.AddTransient<IZastavkaRepository, ZastavkaRepository>();
            services.AddTransient<ZastavkaViewModel>(); // Добавляем эту строку>

            services.AddTransient<IJizdaRepository, JizdaRepository>();
            services.AddTransient<JizdaViewModel>(); // Добавляем эту строку>
            services.AddTransient<SelectRealtionsWindow>();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = ServiceProvider.GetService<MainWindow>();
            var adminWindow = ServiceProvider.GetService<AdminWindow>();
            var employeeWindow = ServiceProvider.GetService<EmployeeWindow>();
        }
    }
}



