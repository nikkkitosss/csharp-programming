using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Windows;

namespace BirthdayApp.Models
{
    public class User : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _birthDate;

        public string FirstName
        {
            get => _firstName;
            set
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Ім'я не може бути порожнє");
                    }

                    if (_firstName != value)
                    {
                        _firstName = value;
                        OnPropertyChanged(nameof(FirstName));
                        UpdateDerived();
                    }
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Ім'я не може бути порожнє", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Прізвище не може бути порожнє");
                    }

                    if (_lastName != value)
                    {
                        _lastName = value;
                        OnPropertyChanged(nameof(LastName));
                        UpdateDerived();
                    }
                }
                catch(ArgumentException)
                {
                    MessageBox.Show("Прізвище не може бути порожнє", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }


        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(value) && !IsValidEmail(value))
                            throw new InvalidEmailException();

                        _email = value;
                        OnPropertyChanged(nameof(Email));
                    }
                    catch (InvalidEmailException)
                    {
                        MessageBox.Show("Невірний формат електронної пошти.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }


        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate != value)
                {
                    try
                    {
                        if (value > DateTime.Today) throw new FutureBirthDateException();
                        if (value < DateTime.Today.AddYears(-135)) throw new TooOldBirthDateException();
                        _birthDate = value;
                        OnPropertyChanged(nameof(BirthDate));
                        UpdateDerived();
                    }
                    catch (FutureBirthDateException)
                    {
                        MessageBox.Show("Дата народження не може бути в майбутньому.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch (TooOldBirthDateException)
                    {
                        MessageBox.Show("Дата народження занадто стара. Введіть реалістичну дату.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }


        public string FullName { get; private set; }
        public int Age { get; private set; }
        public string WesternSign { get; private set; }
        public string ChineseSign { get; private set; }
        public bool IsAdult { get; private set; }
        public bool IsBirthday { get; private set; }

        [JsonConstructor]
        public User()
        {
            _firstName = string.Empty;
            _lastName = string.Empty;
            _email = string.Empty;
            _birthDate = DateTime.Today;
            UpdateDerived();
        }

        public User(string firstName, string lastName, string email, DateTime birthDate)
        {
            _firstName = firstName;
            _lastName = lastName;
            Email = email;
            BirthDate = birthDate;
            UpdateDerived();
        }

        private void UpdateDerived()
        {
            FullName = $"{FirstName} {LastName}";
            Age = CalculateAge();
            WesternSign = CalculateWesternZodiac();
            ChineseSign = CalculateChineseZodiac();
            IsAdult = Age >= 18;
            IsBirthday = BirthDate.Day == DateTime.Today.Day && BirthDate.Month == DateTime.Today.Month;
            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Age));
            OnPropertyChanged(nameof(WesternSign));
            OnPropertyChanged(nameof(ChineseSign));
            OnPropertyChanged(nameof(IsAdult));
            OnPropertyChanged(nameof(IsBirthday));
        }

        private int CalculateAge()
        {
            var age = DateTime.Today.Year - BirthDate.Year;
            if (DateTime.Today < BirthDate.AddYears(age)) age--;
            return age;
        }

        private string CalculateWesternZodiac()
        {
            int day = BirthDate.Day, month = BirthDate.Month;
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

        private string CalculateChineseZodiac()
        {
            string[] animals = { "Мавпа", "Півень", "Собака", "Свиня", "Щур", "Бик", "Тигр", "Кролик", "Дракон", "Змія", "Кінь", "Коза" };
            return animals[BirthDate.Year % 12];
        }

        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
