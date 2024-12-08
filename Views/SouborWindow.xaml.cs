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

        private async void ChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (SouboryViewModel)DataContext;

            await viewModel.SaveChangesAsync();

            foreach (Window window in Application.Current.Windows)
            {
                if (window is SouborWindow)
                {
                    window.Close();
                    break;
                }
            }
        }

        private async void Cancel_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is SouborWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
