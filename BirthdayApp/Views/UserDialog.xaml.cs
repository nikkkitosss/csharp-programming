using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BirthdayApp.Models;

namespace BirthdayApp.Views
{
    public partial class UserDialog : Window
    {
        public User User { get; private set; }

        public UserDialog()
        {
            InitializeComponent();
            User = new User();
            DataContext = User;
        }

        public UserDialog(User user)
        {
            InitializeComponent();
            User = new User(user.FirstName, user.LastName, user.Email, user.BirthDate);
            DataContext = User;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var firstName = FirstName.Text;
            var lastName = LastName.Text;
            var email = Email.Text;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

    }
}

