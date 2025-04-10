using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
<<<<<<< Updated upstream
using System.Windows;
using System.Windows.Input;
=======
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using BirthdayApp.Models;
using BirthdayApp.Views;
using BirthdayApp.Services;
>>>>>>> Stashed changes

namespace BirthdayApp.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
<<<<<<< Updated upstream
        private DateTime _birthDate = DateTime.Today;
        private string _ageText;
        private string _westernZodiac;
        private string _chineseZodiac;
=======
        public ObservableCollection<User> Users { get; set; }
        public ICollectionView UsersView { get; set; }

        private string _filterText;
        private string _selectedFilterProperty;

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                UsersView.Refresh();
            }
        }

        public string SelectedFilterProperty
        {
            get => _selectedFilterProperty;
            set
            {
                _selectedFilterProperty = value;
                OnPropertyChanged(nameof(SelectedFilterProperty));
                UsersView.Refresh();
            }
        }

        public ObservableCollection<string> FilterProperties { get; } = new ObservableCollection<string>
        {
            nameof(User.FirstName), nameof(User.LastName), nameof(User.Email), nameof(User.BirthDate),
            nameof(User.Age), nameof(User.WesternSign), nameof(User.ChineseSign), nameof(User.IsAdult)
        };

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearFilterCommand { get; }

        private void AddUserExecute(object obj)
        {
            AddUser();
        }
>>>>>>> Stashed changes

        private void EditUserExecute(object obj)
        {
<<<<<<< Updated upstream
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }

        public string AgeText
        {
            get => _ageText;
            private set
            {
                _ageText = value;
                OnPropertyChanged(nameof(AgeText));
            }
        }

        public string WesternZodiac
        {
            get => _westernZodiac;
            private set
            {
                _westernZodiac = value;
                OnPropertyChanged(nameof(WesternZodiac));
            }
        }

        public string ChineseZodiac
        {
            get => _chineseZodiac;
            private set
            {
                _chineseZodiac = value;
                OnPropertyChanged(nameof(ChineseZodiac));
            }
        }

        public ICommand ConfirmDateCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            ConfirmDateCommand = new RelayCommand(ConfirmDate);
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void ConfirmDate()
        {
            CalculateAgeAndZodiac();
        }

        private void CalculateAgeAndZodiac()
        {
            int age = DateTime.Today.Year - BirthDate.Year;
            if (BirthDate > DateTime.Today.AddYears(-age)) age--;

            if (age < 0 || age > 135)
            {
                MessageBox.Show("Невірна дата народження!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (BirthDate.Day == DateTime.Today.Day && BirthDate.Month == DateTime.Today.Month)
            {
                MessageBox.Show("Вітаємо з Днем народження!", "Привітання", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            AgeText = $"Вам {age} років.";
            WesternZodiac = GetWesternZodiac(BirthDate);
            ChineseZodiac = GetChineseZodiac(BirthDate);
        }

        private string GetWesternZodiac(DateTime date)
        {
            int day = date.Day, month = date.Month;
            return month switch
            {
                1 => (day <= 19) ? "Козеріг" : "Водолій",
                2 => (day <= 18) ? "Водолій" : "Риби",
                3 => (day <= 20) ? "Риби" : "Овен",
                4 => (day <= 19) ? "Овен" : "Телець",
                5 => (day <= 20) ? "Телець" : "Близнюки",
                6 => (day <= 20) ? "Близнюки" : "Рак",
                7 => (day <= 22) ? "Рак" : "Лев",
                8 => (day <= 22) ? "Лев" : "Діва",
                9 => (day <= 22) ? "Діва" : "Терези",
                10 => (day <= 22) ? "Терези" : "Скорпіон",
                11 => (day <= 21) ? "Скорпіон" : "Стрілець",
                12 => (day <= 21) ? "Стрілець" : "Козеріг",
                _ => "Невідомо"
            };
        }

        private string GetChineseZodiac(DateTime date)
        {
            string[] animals = { "Мавпа", "Півень", "Собака", "Свиня", "Щур", "Бик", "Тигр", "Кролик", "Дракон", "Змія", "Кінь", "Коза" };
            return animals[date.Year % 12];
        }
=======
            EditUser(obj as User);
        }

        private void DeleteUserExecute(object obj)
        {
            DeleteUser(obj as User);
        }

        private bool CanEditOrDeleteUser(object obj)
        {
            return obj is User;
        }

        private void ClearFilterExecute(object obj)
        {
            FilterText = string.Empty;
            SelectedFilterProperty = null;
        }


        public MainViewModel()
        {
            var loaded = DataService.LoadUsers();
            var list = loaded ?? GenerateUsers(50);
            Users = new ObservableCollection<User>(list);
            UsersView = CollectionViewSource.GetDefaultView(Users);
            UsersView.Filter = Filter;
            
            AddCommand = new RelayCommand(AddUserExecute);
            EditCommand = new RelayCommand(EditUserExecute, CanEditOrDeleteUser);
            DeleteCommand = new RelayCommand(DeleteUserExecute, CanEditOrDeleteUser);
            ClearFilterCommand = new RelayCommand(ClearFilterExecute);
        }


        private bool Filter(object obj)
        {
            if (obj is not User user) return false;
            if (string.IsNullOrWhiteSpace(FilterText) || string.IsNullOrEmpty(SelectedFilterProperty)) return true;
            var prop = typeof(User).GetProperty(SelectedFilterProperty);
            var value = prop?.GetValue(user)?.ToString() ?? string.Empty;
            return value.Contains(FilterText, StringComparison.OrdinalIgnoreCase);
        }

        private void AddUser()
        {
            var dlg = new UserDialog();
            if (dlg.ShowDialog() == true)
            {
                Users.Add(dlg.User);
                DataService.SaveUsers(Users);
            }
        }

        private void EditUser(object obj)
        {
            if (obj is not User user) return;

            var dlg = new UserDialog(user);
            if (dlg.ShowDialog() == true)
            {
                user.FirstName = dlg.User.FirstName;
                user.LastName = dlg.User.LastName;
                user.Email = dlg.User.Email;
                user.BirthDate = dlg.User.BirthDate;

                DataService.SaveUsers(Users);
                UsersView.Refresh();
            }
        }

        private void DeleteUser(object obj)
        {
            if (obj is not User user) return;
            Users.Remove(user);
            DataService.SaveUsers(Users);
        }

        private static List<User> GenerateUsers(int count)
        {
            var rnd = new Random();
            var firstNames = new[] { "Ivan", "Olena", "Mykhailo", "Maria", "Serhii", "Kateryna" };
            var lastNames = new[] { "Shevchenko", "Kovalenko", "Bondar", "Tkach", "Kovalchuk" };
            var list = new List<User>();
            for (int i = 0; i < count; i++)
            {
                var fn = firstNames[rnd.Next(firstNames.Length)];
                var ln = lastNames[rnd.Next(lastNames.Length)];
                var bd = DateTime.Today.AddYears(-rnd.Next(1, 80)).AddDays(rnd.Next(0, 365));
                var email = $"{fn.ToLower()}.{ln.ToLower()}{rnd.Next(100, 999)}@example.com";
                list.Add(new User(fn, ln, email, bd));
            }
            DataService.SaveUsers(list);
            return list;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
>>>>>>> Stashed changes
    }
}
