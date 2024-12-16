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

            EditedAddress = selectedAddress ?? new Adresa(); 
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
            if (EditedAddress == null)
            {
                MessageBox.Show("No address data available.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (EditedAddress.AdresaId == 0) 
                {
                    int newAddressId = await _adresyRepository.AddAddressAsync(
                        EditedAddress.Mesto,
                        EditedAddress.Ulice,
                        EditedAddress.CisloBudovy,
                        EditedAddress.ZipCode,
                        EditedAddress.CisloBytu
                    );

                    OriginalZamestnanec.AdresaId = newAddressId; 
                    MessageBox.Show("New address added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    await _adresyRepository.UpdateAddressLogicAsync(
                        EditedAddress,
                        OriginalZamestnanec.ZamestnanecId,
                        OriginalZamestnanec.AdresaId
                    );
                    MessageBox.Show("Address updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
