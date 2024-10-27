using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DopravniPodnikSem.Models.Enum;

namespace DopravniPodnikSem.Models
{
    public class UserData : INotifyPropertyChanged
    {
        private int userId;
        private string email;
        private int binaryContentIdContent;
        private Role role;

        public int UserId
        {
            get { return userId; }
            set
            {
                if (userId != value)
                {
                    userId = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged();
                }
            }
        }
        public int IdContent
        {
            get { return binaryContentIdContent; }
            set
            {
                if (binaryContentIdContent != value)
                {
                    binaryContentIdContent = value;
                    OnPropertyChanged();
                }
            }
        }
        public Role RoleUser
        {
            get { return role; }
            set
            {
                if (role != value)
                {
                    role = value;
                    OnPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
