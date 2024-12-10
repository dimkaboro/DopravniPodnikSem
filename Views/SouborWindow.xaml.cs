using DopravniPodnikSem.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace DopravniPodnikSem.Views
{
    /// <summary>
    /// Логика взаимодействия для SouborWindow.xaml
    /// </summary>
    public partial class SouborWindow : Window
    {
        public SouborWindow()
        {
            InitializeComponent();
        }

        private void ChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var imageBytes = File.ReadAllBytes(filePath);

                if (DataContext is SouboryViewModel viewModel)
                {
                    viewModel.NewAvatar = imageBytes;
                    MessageBox.Show("Новый аватар выбран.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ошибка загрузки аватара.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void SaveAvatar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SouboryViewModel viewModel)
            {
                await viewModel.SaveChangesAsync();
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
