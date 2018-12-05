using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientEmployees.Model
{
    public class EmployeeModel : INotifyPropertyChanged
    {
        private int _id;
        private string _firstName;
        private string _lastName;
        private string _address;
        private string _homeTelephone;
        private string _mobileTelephone;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        public string LastName
        {
            get
            {
                return _lastName; 
            }
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }
        public string HomeTelephone
        {
            get
            {
                return _homeTelephone;
            }
            set
            {
                _homeTelephone = value;
                OnPropertyChanged("HomeTelephone");
            }
        }
        public string MobileTelephone
        {
            get
            {
                return _mobileTelephone;
            }
            set
            {
                _mobileTelephone = value;
                OnPropertyChanged("MobileTelephone");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
