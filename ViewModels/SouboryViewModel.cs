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

        public SouboryViewModel(ISouboryRepository souboryRepository, Zamestnanec selectedZamestnanec, Soubory selectedSoubor)
        {
            _souboryRepository = souboryRepository;
            _originalZamestnanec = selectedZamestnanec;
            CurrentSoubor = selectedSoubor ?? throw new ArgumentNullException(nameof(selectedSoubor));
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

        public async Task SaveChangesAsync()
        {
            try
            {
                if (NewAvatar != null)
                {
                    var newAvatarId = await _souboryRepository.UpdateUserAvatarAsync(OriginalZamestnanec.ZamestnanecId, "User avatar", NewAvatar);
                    MessageBox.Show($"Avatar byl úspěšně aktualizován!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Nebyl vybrán žádný nový avatar. Je použit standardní avatar.", "Informace", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba aktualizace dat: {ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
