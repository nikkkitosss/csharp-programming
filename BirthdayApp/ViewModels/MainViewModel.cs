using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace BirthdayApp.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private DateTime _birthDate = DateTime.Today;
        private string _ageText;
        private string _westernZodiac;
        private string _chineseZodiac;

        public DateTime BirthDate
        {
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
    }
}
