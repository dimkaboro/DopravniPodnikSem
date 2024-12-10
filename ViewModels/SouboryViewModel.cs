using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository;
using DopravniPodnikSem.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DopravniPodnikSem.ViewModels
{
    public class SouboryViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly ISouboryRepository _souboryRepository;

        private Zamestnanec _originalZamestnanec;
        private Soubory _currentSoubor;
        private byte[] _newAvatar;

        private const int DefaultAvatarId = 1; // ID дефолтного аватара

        public SouboryViewModel(ISouboryRepository souboryRepository, Zamestnanec selectedZamestnanec, Soubory selectedSoubor)
        {
            _souboryRepository = souboryRepository;
            _originalZamestnanec = selectedZamestnanec;
            CurrentSoubor = selectedSoubor ?? throw new ArgumentNullException(nameof(selectedSoubor));

            LoadDefaultAvatarAsync().Wait(); // Загружаем дефолтный аватар при инициализации
        }

        public Zamestnanec OriginalZamestnanec
        {
            get => _originalZamestnanec;
            set
            {
                _originalZamestnanec = value;
                OnPropertyChanged(nameof(OriginalZamestnanec));
            }
        }

        public Soubory CurrentSoubor
        {
            get => _currentSoubor;
            set
            {
                _currentSoubor = value;
                OnPropertyChanged(nameof(CurrentSoubor));
                OnPropertyChanged(nameof(CurrentAvatar));
            }
        }

        public byte[] NewAvatar
        {
            get => _newAvatar;
            set
            {
                _newAvatar = value;
                OnPropertyChanged(nameof(NewAvatar));
                OnPropertyChanged(nameof(CurrentAvatar));
            }
        }

        public byte[] CurrentAvatar => NewAvatar ?? CurrentSoubor?.Soubor;

        private async Task LoadDefaultAvatarAsync()
        {
            try
            {
                _currentSoubor = await _souboryRepository.GetUserAvatarAsync(DefaultAvatarId);
                OnPropertyChanged(nameof(CurrentSoubor));
                OnPropertyChanged(nameof(CurrentAvatar));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки аватара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                if (NewAvatar != null)
                {
                    var newAvatarId = await _souboryRepository.UpdateUserAvatarAsync(OriginalZamestnanec.ZamestnanecId, "User avatar", NewAvatar);
                    MessageBox.Show($"Аватар успешно обновлён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Новый аватар не выбран. Используется стандартный аватар.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
