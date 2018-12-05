using AccessToWebApi;
using AccessToWebApi.Entities;
using ClientEmployees.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ClientEmployees.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private DispatcherTimer _timer;       
        private HTTPForEmployee _employeeHttpClient;
       
        public ObservableCollection<EmployeeModel> Employees { get; set; }

        
        public EmployeeViewModel(HTTPForEmployee employeeHttpClient)
        {
            Employees = new ObservableCollection<EmployeeModel>();
            _employeeHttpClient = employeeHttpClient;
            

            _timer = new System.Windows.Threading.DispatcherTimer();

            _timer.Tick += new EventHandler(TimerTick);
            _timer.Interval = new TimeSpan(0, 0, 2);
            _timer.Start();

        }

        private void TimerTick(object sender, EventArgs e)
        {
            try
            {
                List<Employee> employeesFromServer = _employeeHttpClient.Get().ToList();


                /*
                 * Необходимо проверить текущий список и полученный от сервера на расхождения.
                 * Если Списки разные,то текущий список обновляется.
                 */
                bool flagUpdate = false;//Изначально обновлять не надо

                if (Employees.Count() == employeesFromServer.Count())
                {
                    for (int i = 0; i < Employees.Count; i++)
                    {
                        if (Employees[i].Id != employeesFromServer[i].Id)//есть расхождения
                        {
                            flagUpdate = true;//обновляем
                            break;
                        }
                    }
                }
                else
                {
                    flagUpdate = true;//обновляем
                }

                //Обновление текущего списка
                if (flagUpdate)
                {
                    Employees.Clear();
                    foreach (var employee in employeesFromServer)
                        Employees.Add(new EmployeeModel()
                        {
                            Id = employee.Id,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Address = employee.Address,
                            HomeTelephone = employee.HomeTelephone,
                            MobileTelephone = employee.MobileTelephone
                        });
                }
            }
            catch
            {
                MessageBox.Show("Нет соединения с сервером. Перезапустите программу.");
                _timer.Stop();
            }
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        
        public event PropertyChangedEventHandler PropertyChanged;


        private int _filterId;
        private string _filterFirstName;
        private string _filterLastName;
        private string _filterAddress;
        private string _filterHomeTelephone;
        private string _filterMobileTelephone;

        public int FilterId
        {
            get
            {
                return _filterId;
            }
            set
            {
                _filterId = value;
                OnPropertyChanged("FilterId");
            }
        }
        public string FilterFirstName
        {
            get
            {
                return _filterFirstName;
            }
            set
            {
                _filterFirstName = value;
                OnPropertyChanged("FilterFirstName");
            }
        }
        public string FilterLastName
        {
            get
            {
                return _filterLastName;
            }
            set
            {
                _filterLastName = value;
                OnPropertyChanged("FilterLastName");
            }
        }
        public string FilterAddress
        {
            get
            {
                return _filterAddress;
            }
            set
            {
                _filterAddress = value;
                OnPropertyChanged("FilterAddress");
            }
        }
        public string FilterHomeTelephone
        {
            get
            {
                return _filterHomeTelephone;
            }
            set
            {
                _filterHomeTelephone = value;
                OnPropertyChanged("FilterHomeTelephone");
            }
        }
        public string FilterMobileTelephone
        {
            get
            {
                return _filterMobileTelephone;
            }
            set
            {
                _filterMobileTelephone = value;
                OnPropertyChanged("FilterMobileTelephone");
            }
        }

        private CommandHandler _filteredEmployees;
        public CommandHandler FilteredEmployees
        {
            get
            {
                return _filteredEmployees ??
            (_filteredEmployees = new CommandHandler(obj =>
            {
                _timer.Stop();
                List<Func<Employee, bool>> predicates=new List<Func<Employee, bool>>();

                bool flagFilter = false;
                if (FilterId > 1)
                {
                    flagFilter = true;
                    predicates.Add(x => { return x.Id.ToString().StartsWith(FilterId.ToString()); });
                }
                if (!string.IsNullOrEmpty(FilterFirstName))
                {
                    flagFilter = true;
                    predicates.Add(x => { return x.FirstName.StartsWith(FilterFirstName); });
                }
                if (!string.IsNullOrEmpty(FilterLastName))
                {
                    flagFilter = true;
                    predicates.Add(x => { return x.LastName.StartsWith(FilterLastName); });
                }
                if (!string.IsNullOrEmpty(FilterAddress))
                {
                    flagFilter = true;
                    predicates.Add(x => { return x.Address.StartsWith(FilterAddress); });
                }
                if (!string.IsNullOrEmpty(FilterHomeTelephone))
                {
                    flagFilter = true;
                    predicates.Add(x => { return x.HomeTelephone.StartsWith(FilterHomeTelephone); });
                }
                if (!string.IsNullOrEmpty(FilterMobileTelephone))
                {
                    flagFilter = true;
                    predicates.Add(x => { return x.MobileTelephone.StartsWith(FilterMobileTelephone); });
                }

                if (flagFilter)
                {
                    IEnumerable<Employee> employeesFromServer = _employeeHttpClient.Get();
                    foreach(var predicate in predicates)
                    {
                        employeesFromServer = employeesFromServer.Where(predicate);
                    }

                    Employees.Clear();
                    foreach (var employee in employeesFromServer)
                        Employees.Add(new EmployeeModel()
                        {
                            Id = employee.Id,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Address = employee.Address,
                            HomeTelephone = employee.HomeTelephone,
                            MobileTelephone = employee.MobileTelephone
                        });

                }     
            },
            (obj) =>
            {              
                return true;
            }
            ));
            }


        }

        private CommandHandler _clearFilter;
        public CommandHandler ClearFilter
        {
            get
            {
                return _clearFilter ??
            (_clearFilter = new CommandHandler(obj =>
            {
                _timer.Start();
            },
            (obj) =>
            {
                return true;
            }
            ));
            }
        }


       
        private string _newFirstName;
        private string _newLastName;
        private string _newAddress;
        private string _newHomeTelephone;
        private string _newMobileTelephone;

        
        public string NewFirstName
        {
            get
            {
                return _newFirstName;
            }
            set
            {
                _newFirstName = value;
                OnPropertyChanged("NewFirstName");
            }
        }
        public string NewLastName
        {
            get
            {
                return _newLastName;
            }
            set
            {
                _newLastName = value;
                OnPropertyChanged("NewLastName");
            }
        }
        public string NewAddress
        {
            get
            {
                return _newAddress;
            }
            set
            {
                _newAddress = value;
                OnPropertyChanged("NewAddress");
            }
        }
        public string NewHomeTelephone
        {
            get
            {
                return _newHomeTelephone;
            }
            set
            {
                _newHomeTelephone = value;
                OnPropertyChanged("NewHomeTelephone");
            }
        }
        public string NewMobileTelephone
        {
            get
            {
                return _newMobileTelephone;
            }
            set
            {
                _newMobileTelephone = value;
                OnPropertyChanged("NewMobileTelephone");
            }
        }


        private CommandHandler _newEmployees;
        public CommandHandler NewEmployees
        {
            get
            {
                return _newEmployees ??
            (_newEmployees = new CommandHandler(obj =>
            {
                if(!string.IsNullOrEmpty(NewFirstName) && !string.IsNullOrEmpty(NewLastName) && !string.IsNullOrEmpty(NewAddress)  && !string.IsNullOrEmpty(NewMobileTelephone))
                {
                    Employee newEmployee = new Employee()
                    {
                       
                        FirstName = NewFirstName,
                        LastName = NewLastName,
                        Address = NewAddress,
                        HomeTelephone = NewHomeTelephone,
                        MobileTelephone = NewMobileTelephone
                    };

                    var newEmployeeFromServer = _employeeHttpClient.Post(newEmployee);

                    if (newEmployeeFromServer == null)
                    {
                        MessageBox.Show("Сотрудник не был добавлен");
                    }
                    else
                    {
                        MessageBox.Show("Сотрудник был добавлен");
                    }
                }
            },
            (obj) =>
            {
                return (!string.IsNullOrEmpty(NewFirstName) && !string.IsNullOrEmpty(NewLastName) && !string.IsNullOrEmpty(NewAddress) && !string.IsNullOrEmpty(NewMobileTelephone)) ;
            }
            ));
            }
        }
    }
}
