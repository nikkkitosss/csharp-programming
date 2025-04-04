using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BirthdayApp;
using BirthdayApp.Models;

namespace BirthdayApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Person _person;
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _birthDate = DateTime.Today;
        private bool _isButtonEnabled = true;
        private string _outputFullName;
        private string _ageText;
        private string _westernSign;
        private string _chineseSign;
        private string _outputBirthDate;
        private string _outputEmail;
        private string _outputIsAdult;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); OnPropertyChanged(nameof(CanProceed)); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); OnPropertyChanged(nameof(CanProceed)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); OnPropertyChanged(nameof(CanProceed)); }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set { _birthDate = value; OnPropertyChanged(nameof(BirthDate)); OnPropertyChanged(nameof(CanProceed)); }
        }

        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            private set { _isButtonEnabled = value; OnPropertyChanged(nameof(IsButtonEnabled)); }
        }

        public bool CanProceed =>
            !string.IsNullOrWhiteSpace(FirstName) &&
            !string.IsNullOrWhiteSpace(LastName) &&
            !string.IsNullOrWhiteSpace(Email) &&
            BirthDate != DateTime.MinValue;

        public string OutputFullName => _outputFullName;
        public string AgeText => _ageText;
        public string WesternSign => _westernSign;
        public string ChineseSign => _chineseSign;
        public string OutputBirthDate => _outputBirthDate;
        public string OutputEmail => _outputEmail;
        public string OutputIsAdult => _outputIsAdult;

        public ICommand ProceedCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            ProceedCommand = new RelayCommand(async () => await Proceed(), () => CanProceed);
        }

        private async Task Proceed()
        {
            IsButtonEnabled = false;
            try
            {
                _person = new Person(FirstName, LastName, Email, BirthDate);
                await Task.Run(CalculateAgeAndZodiac);
                OnPropertyChanged(nameof(OutputFullName));
                OnPropertyChanged(nameof(AgeText));
                OnPropertyChanged(nameof(WesternSign));
                OnPropertyChanged(nameof(ChineseSign));
                OnPropertyChanged(nameof(OutputBirthDate));
                OnPropertyChanged(nameof(OutputEmail));
                OnPropertyChanged(nameof(OutputIsAdult));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsButtonEnabled = true;
            }
        }

        private void CalculateAgeAndZodiac()
        {
            if (_person.Age < 0 || _person.Age > 135)
            {
                MessageBox.Show("Невірна дата народження!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_person.IsBirthday)
            {
                MessageBox.Show("Вітаємо з Днем народження!", "Привітання", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            _outputFullName = $"Прізвище ім'я: {_person.FullName}";
            _ageText = $"Вік: {_person.Age}";
            _westernSign = $"Західний знак зодіаку: {_person.WesternSign}";
            _chineseSign = $"Китайський знак зодіаку: {_person.ChineseSign}";
            _outputBirthDate = $"Дата народження: {_person.BirthDate.ToShortDateString()}";
            _outputEmail = $"Email: {_person.Email}";
            _outputIsAdult = $"Дорослий: {(_person.IsAdult ? "Так" : "Ні")}";
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
