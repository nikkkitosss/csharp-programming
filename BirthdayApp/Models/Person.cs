using System;
using System.Text.RegularExpressions;

namespace BirthdayApp.Models
{
    class FutureBirthDateException : Exception
    {
        public FutureBirthDateException() : base("Дата народження не може бути в майбутньому.") { }
    }

    class TooOldBirthDateException : Exception
    {
        public TooOldBirthDateException() : base("Дата народження занадто далека в минулому. Підтримуються лише живі люди.") { }
    }

    class InvalidEmailException : Exception
    {
        public InvalidEmailException() : base("Невірний формат електронної пошти.") { }
    }

    class Person
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; }
        public DateTime BirthDate { get; }

        private readonly string _westernSign;
        private readonly string _chineseSign;
        private readonly bool _isAdult;
        private readonly bool _isBirthday;
        private readonly int _age;

        public string WesternSign => _westernSign;
        public string ChineseSign => _chineseSign;
        public bool IsAdult => _isAdult;
        public bool IsBirthday => _isBirthday;
        public int Age => _age;

        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            if (birthDate > DateTime.Today)
                throw new FutureBirthDateException();
            if (birthDate < DateTime.Today.AddYears(-135))
                throw new TooOldBirthDateException();
            if (!IsValidEmail(email))
                throw new InvalidEmailException();

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;

            _westernSign = CalculateWesternZodiac();
            _chineseSign = CalculateChineseZodiac();
            _isAdult = CalculateIsAdult();
            _isBirthday = CalculateIsBirthday();
            _age = CalculateAge();
        }

        public Person(string firstName, string lastName, string email)
            : this(firstName, lastName, email, DateTime.Today) { }

        public Person(string firstName, string lastName, DateTime birthDate)
            : this(firstName, lastName, string.Empty, birthDate) { }

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

        private bool CalculateIsAdult()
        {
            return (DateTime.Today - BirthDate).TotalDays / 365.25 >= 18;
        }

        private bool CalculateIsBirthday()
        {
            return BirthDate.Day == DateTime.Today.Day && BirthDate.Month == DateTime.Today.Month;
        }

        private int CalculateAge()
        {
            return DateTime.Today.Year - BirthDate.Year - (DateTime.Today < BirthDate.AddYears(DateTime.Today.Year - BirthDate.Year) ? 1 : 0);
        }

        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$");
        }
    }

}