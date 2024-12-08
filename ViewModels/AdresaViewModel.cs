using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository;
using DopravniPodnikSem.Repository.Interfaces;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace DopravniPodnikSem.ViewModels
{
    public class AdresaViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly IAdresyRepository _adresyRepository;

        private Zamestnanec _originalZamestnanec;
        private Adresa _editedAddress;

        public AdresaViewModel(IAdresyRepository adresyRepository, Zamestnanec selectedZamestnanec, Adresa selectedAddress)
        {
            _adresyRepository = adresyRepository;

            _originalZamestnanec = selectedZamestnanec;

            EditedAddress = new Adresa
            {
                AdresaId = selectedAddress.AdresaId,
                Mesto = selectedAddress.Mesto,
                Ulice = selectedAddress.Ulice,
                CisloBudovy = selectedAddress.CisloBudovy,
                ZipCode = selectedAddress.ZipCode,
                CisloBytu = selectedAddress.CisloBytu
            };
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

        public Adresa EditedAddress
        {
            get => _editedAddress;
            set
            {
                _editedAddress = value;
                OnPropertyChanged(nameof(EditedAddress));
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _adresyRepository.UpdateAddressLogicAsync(EditedAddress, OriginalZamestnanec.ZamestnanecId, OriginalZamestnanec.AdresaId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
