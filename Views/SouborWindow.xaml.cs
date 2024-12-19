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
using System.IO;
using Microsoft.Win32;

namespace DopravniPodnikSem.Views
{
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
                Title = "Select an image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var imageBytes = File.ReadAllBytes(filePath);
                var fileExtension = Path.GetExtension(filePath)?.ToLower();

                if (DataContext is SouboryViewModel viewModel)
                {
                    viewModel.NewAvatar = imageBytes;
                    viewModel.NewAvatarExtension = fileExtension;
                }
                else
                {
                    MessageBox.Show("Error loading avatar.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
