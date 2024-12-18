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

            services.AddSingleton<NavigationVM>();
            services.AddTransient<DatabaseService>();
            services.AddTransient<PasswordService>();

            services.AddTransient<IUserDataRepository, UserDataRepository>();
            services.AddTransient<IAdresyRepository, AdresyRepository>();
            services.AddTransient<ISouboryRepository, SouboryRepository>();
            services.AddTransient<ILogRepository, LogRepository>();

            services.AddTransient<RegistrationViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<EditProfileViewModel>();
            services.AddTransient<UserEmulationViewModel>();
            services.AddTransient<ShowUsersViewModel>();
            services.AddTransient<LogViewModel>();

            services.AddTransient<ZamestnanecViewModel>();
            services.AddTransient<AdresaViewModel>();
            services.AddTransient<SouboryViewModel>();
            services.AddTransient<AdresaWindow>();
            services.AddTransient<SouborWindow>();

            services.AddTransient<IVozidloRepository, VozidloRepository>();
            services.AddTransient<VozidloViewModel>();

            services.AddTransient<IRidiciRepository, RidiciRepository>();
            services.AddTransient<RidiciViewModel>();

            services.AddTransient<ILinkyRepository, LinkyRepository>();
            services.AddTransient<LinkaViewModel>();

            services.AddTransient<IUdrzbaVozidlaRepository, UdrzbaVozidlaRepository>();
            services.AddTransient<UdrzbaVozidlaViewModel>();

            services.AddTransient<UdrzbaVozidlaViewModel>();
            services.AddTransient<SearchVozidloViewModel>();
            services.AddTransient<SearchVozidloWindow>();

            services.AddTransient<IZastavkaRepository, ZastavkaRepository>();
            services.AddTransient<ZastavkaViewModel>();

            services.AddTransient<IJizdaRepository, JizdaRepository>();
            services.AddTransient<JizdaViewModel>();
            services.AddTransient<SelectRealtionsWindow>();

            services.AddTransient<IDopravniPlatbyRepository, DopravniPlatbyRepository>();
            services.AddTransient<DopravniPlatbyViewModel>();
            services.AddTransient<TypePlatbaView>();

            services.AddTransient<TypePlatbaViewModel>();

            services.AddTransient<ISystemKatalogRepository, SystemKatalogRepository>();
            services.AddTransient<SystemKatalogViewModel>();

            services.AddTransient<IZastavkyTrasyRepository, ZastavkyTrasyRepository>();
            services.AddScoped<ZastavkyTrasyViewModel>();

            services.AddTransient<AddZastavkaTrasaView>();
            services.AddTransient<AddZastavkaTrasaViewModel>();
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



