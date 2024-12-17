using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class SystemKatalog : BaseModel
    {
        private string _OWNER;
        private string _OBJECT_NAME;
        private int _OBJECT_ID;
        private string _CREATED;
        private string _STATUS;
        private string _OBJECT_TYPE;
        private string _LAST_DDL_TIME;

        public string OWNER
        {
            get => _OWNER;
            set => SetField(ref _OWNER, value);
        }

        public string OBJECT_NAME
        {
            get => _OBJECT_NAME;
            set => SetField(ref _OBJECT_NAME, value);
        }

        public int OBJECT_ID
        {
            get => _OBJECT_ID;
            set => SetField(ref _OBJECT_ID, value);
        }

        public string CREATED
        {
            get => _CREATED;
            set => SetField(ref _CREATED, value);
        }

        public string STATUS
        {
            get => _STATUS;
            set => SetField(ref _STATUS, value);
        }

        public string OBJECT_TYPE
        {
            get => _OBJECT_TYPE;
            set => SetField(ref _OBJECT_TYPE, value);
        }

        public string LAST_DDL_TIME
        {
            get => _LAST_DDL_TIME;
            set => SetField(ref _LAST_DDL_TIME, value);
        }
    }
}


