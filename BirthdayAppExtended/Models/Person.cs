using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayAppExtended.Models
{
    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }
        public DateTime BirthDate { get; set; }

        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            Initialize(firstName, lastName, email, birthDate);
        }

        public Person(string firstName, string lastName, string email)
            : this(firstName, lastName, email, DateTime.Today) 
        { }

        public Person(string firstName, string lastName, DateTime birthDate)
            : this(firstName, lastName, string.Empty, birthDate) 
        { }

        private void Initialize(string firstName, string lastName, string email, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;
        }

        public string WesternSign => GetWesternZodiac(BirthDate);
        public string ChineseSign => GetChineseZodiac(BirthDate);
        public bool IsAdult => (DateTime.Now - BirthDate).TotalDays / 365.25 >= 18;
        public bool IsBirthday => BirthDate.Day == DateTime.Today.Day && BirthDate.Month == DateTime.Today.Month;
        public int Age => DateTime.Today.Year - BirthDate.Year - (DateTime.Today < BirthDate.AddYears(DateTime.Today.Year - BirthDate.Year) ? 1 : 0);

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
