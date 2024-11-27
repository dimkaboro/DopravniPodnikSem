using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.ViewModels
{
    public class SouboryViewModel : BaseViewModel
    {
        private Soubory _soubory;

        public SouboryViewModel(Soubory soubory)
        {
            _soubory = soubory;
        }

        public int SouborId
        {
            get => _soubory.SouborId;
            set
            {
                _soubory.SouborId = value;
                OnPropertyChanged();
            }
        }

        public string Nazev
        {
            get => _soubory.Nazev;
            set
            {
                _soubory.Nazev = value;
                OnPropertyChanged();
            }
        }

        // Для работы с бинарными данными файла можно хранить путь к файлу или использовать byte[].
        // Если вам нужно работать с файлом в приложении, добавьте:
        public byte[] Soubor
        {
            get => _soubory.Soubor;
            set
            {
                _soubory.Soubor = value;
                OnPropertyChanged();
            }
        }
    }
}
