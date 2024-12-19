using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace DopravniPodnikSem.Views
{
    public partial class EditProfileView : UserControl
    {
        private readonly NavigationVM _navigationVM;

        public EditProfileView()
        {
            InitializeComponent();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var databaseService = new DatabaseService(configuration);

            _navigationVM = App.ServiceProvider.GetService<NavigationVM>();
        }

        private async void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Select an Avatar"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                var imageBytes = await File.ReadAllBytesAsync(filePath);

                var fileExtension = Path.GetExtension(filePath)?.ToLower();

                var viewModel = (EditProfileViewModel)DataContext;
                viewModel.NewAvatar = imageBytes;
                viewModel.NewAvatarExtension = fileExtension;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (EditProfileViewModel)DataContext;

            await viewModel.SaveChangesAsync();

            var updatedUser = await App.ServiceProvider.GetService<IUserDataRepository>()
                                   .GetUserDetailsAsync(CurrentSession.LoggedInUser.ZamestnanecId);
            CurrentSession.LoggedInUser = updatedUser;

            var navigationVM = App.ServiceProvider.GetService<NavigationVM>();
            navigationVM.CurrentView = new ProfileView
            {
                DataContext = new ProfileViewModel(
                    App.ServiceProvider.GetService<IUserDataRepository>(),
                    App.ServiceProvider.GetService<IAdresyRepository>(),
                    App.ServiceProvider.GetService<ISouboryRepository>(),
                    CurrentSession.LoggedInUser.ZamestnanecId,
                    CurrentSession.LoggedInUser.AdresaId,
                    CurrentSession.LoggedInUser.SouborId)
            };
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (EditProfileViewModel)DataContext;
            viewModel.CancelChanges();

            var navigationVM = App.ServiceProvider.GetService<NavigationVM>();
            navigationVM.CurrentView = new ProfileView
            {
                DataContext = new ProfileViewModel(
                    App.ServiceProvider.GetService<IUserDataRepository>(),
                    App.ServiceProvider.GetService<IAdresyRepository>(),
                    App.ServiceProvider.GetService<ISouboryRepository>(),
                    CurrentSession.LoggedInUser.ZamestnanecId,
                    CurrentSession.LoggedInUser.AdresaId,
                    CurrentSession.LoggedInUser.SouborId)
            };
        }
    }
}
