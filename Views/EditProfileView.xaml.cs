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
using System.Windows.Shapes;

namespace DopravniPodnikSem.Views
{
    /// <summary>
    /// Логика взаимодействия для EditProfileView.xaml
    /// </summary>
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
            // Открытие диалога выбора файла
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Select an Avatar"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Загружаем выбранное изображение
                string filePath = openFileDialog.FileName;

                // Читаем содержимое файла как массив байтов
                var imageBytes = await File.ReadAllBytesAsync(filePath);

                // Устанавливаем новое изображение во ViewModel
                var viewModel = (EditProfileViewModel)DataContext;
                viewModel.NewAvatar = imageBytes;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (EditProfileViewModel)DataContext;

            // Сохраняем изменения
            await viewModel.SaveChangesAsync();

            var updatedUser = await App.ServiceProvider.GetService<IUserDataRepository>()
                                   .GetUserDetailsAsync(CurrentSession.LoggedInUser.ZamestnanecId);
            CurrentSession.LoggedInUser = updatedUser;

            // Возвращаемся в ProfileView
            var navigationVM = App.ServiceProvider.GetService<NavigationVM>();
            navigationVM.CurrentView = new ProfileView
            {
                DataContext = new ProfileViewModel(
                    App.ServiceProvider.GetService<IUserDataRepository>(),
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
                    CurrentSession.LoggedInUser.ZamestnanecId,
                    CurrentSession.LoggedInUser.AdresaId,
                    CurrentSession.LoggedInUser.SouborId)
            };
        }
    }
}
