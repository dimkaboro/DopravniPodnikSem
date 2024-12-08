using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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
   
    public partial class AdresaWindow : Window
    {
        public AdresaWindow()
        {
            InitializeComponent();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (AdresaViewModel)DataContext;

            await viewModel.SaveChangesAsync();

            foreach (Window window in Application.Current.Windows)
            {
                if (window is AdresaWindow)
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
                if (window is AdresaWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
