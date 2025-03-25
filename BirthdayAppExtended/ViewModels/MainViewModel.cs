using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BirthdayAppExtended.Models;

namespace BirthdayAppExtended.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Person _person;
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _birthDate = DateTime.Today;
        private string _outputFullName;
        private string _ageText;
        private string _westernSign;
        private string _chineseSign;
        private string _outputBirthDate;
        private string _outputEmail;
        private string _outputIsAdult;
        private bool _isButtonEnabled;

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                OnPropertyChanged(nameof(CanProceed));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                OnPropertyChanged(nameof(CanProceed));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(CanProceed));
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
                OnPropertyChanged(nameof(CanProceed));
            }
        }

        public string OutputFullName
        {
            get => _outputFullName;
            private set
            {
                _outputFullName = value;
                OnPropertyChanged(nameof(OutputFullName));
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

        public string WesternSign
        {
            get => _westernSign;
            private set
            {
                _westernSign = value;
                OnPropertyChanged(nameof(WesternSign));
            }
        }

        public string ChineseSign
        {
            get => _chineseSign;
            private set
            {
                _chineseSign = value;
                OnPropertyChanged(nameof(ChineseSign));
            }
        }

        public string OutputBirthDate
        {
            get => _outputBirthDate;
            private set
            {
                _outputBirthDate = value;
                OnPropertyChanged(nameof(OutputBirthDate));
            }
        }

        public string OutputEmail
        {
            get => _outputEmail;
            private set
            {
                _outputEmail = value;
                OnPropertyChanged(nameof(OutputEmail));
            }
        }

        public string OutputIsAdult
        {
            get => _outputIsAdult;
            private set
            {
                _outputIsAdult = value;
                OnPropertyChanged(nameof(OutputIsAdult));
            }
        }

        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set
            {
                _isButtonEnabled = value;
                OnPropertyChanged(nameof(IsButtonEnabled));
            }
        }

        public bool CanProceed =>
            !string.IsNullOrWhiteSpace(FirstName) &&
            !string.IsNullOrWhiteSpace(LastName) &&
            !string.IsNullOrWhiteSpace(Email) &&
            BirthDate != DateTime.MinValue;

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
                await Task.Run(() => CalculateAgeAndZodiac());
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
            OutputFullName = $"Прізвище ім'я: {_person.FullName}";
            AgeText = $"Вік: {_person.Age}";
            WesternSign = $"Західний знак зодіаку: {_person.WesternSign}";
            ChineseSign = $"Китайський знак зодіаку: {_person.ChineseSign}";
            OutputBirthDate = $"Дата народження: {_person.BirthDate.ToShortDateString()}";
            OutputEmail = $"Email: {_person.Email}";
            OutputIsAdult = $"Дорослий: {(_person.IsAdult ? "Так" : "Ні")}";
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
