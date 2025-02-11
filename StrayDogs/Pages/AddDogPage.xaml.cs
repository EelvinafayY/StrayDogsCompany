using StrayDogs.DB;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StrayDogs.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddDogPage.xaml
    /// </summary>
    public partial class AddDogPage : Page
    {
        public static Dog dog = new Dog();
        public static List<Gender> genders {  get; set; }
        public static List<Aviary> aviaries { get; set; }

        public AddDogPage()
        {
            InitializeComponent();
            genders = DBConnection.stray_DogsEntities.Gender.ToList();
            aviaries = DBConnection.stray_DogsEntities.Aviary.ToList();
        }

        private void AddBTN_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (string.IsNullOrWhiteSpace(NumberTB.Text) || string.IsNullOrWhiteSpace(HeightTB.Text) ||
            //           string.IsNullOrWhiteSpace(WeightTB.Text) || string.IsNullOrWhiteSpace(AgeTB.Text) ||
            //           string.IsNullOrWhiteSpace(DescriptionTB.Text) || GenderCB.SelectedItem == null || VolierCB.SelectedItem == null)
            //    {
            //        MessageBox.Show("Заполните все поля.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            //        return;
            //    }
            //    else
            //    {
            //        dog.Surname = SurnameTB.Text.Trim();
            //        dog.Name = NameTB.Text.Trim();
            //        dog.Patronymic = PatronymicTB.Text.Trim();
            //        dog.DateOfBirth = DateOfBirthDP.SelectedDate;

            //        string login = LoginTB.Text;
            //        bool loginExists = DBConnection.massageSalon.dog.Any(w => w.Login == login);

            //        if (loginExists)
            //        {
            //            MessageBox.Show("Этот логин уже занят. Выберите другой.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            //            return;
            //        }
            //        else
            //        {
            //            if (LoginTB.Text.Length > 13)
            //            {
            //                MessageBox.Show("Слишком длинный логин.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            //                return;
            //            }
            //            else if (LoginTB.Text.Length < 6)
            //            {
            //                MessageBox.Show("Слишком короткий логин.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            //                return;
            //            }
            //            else
            //            {
            //                dog.Login = LoginTB.Text.Trim();
            //            }
            //        }

            //        if (PasswordTB.Text.Length > 13)
            //        {
            //            MessageBox.Show("Слишком длинный пароль.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            //            return;
            //        }
            //        else if (PasswordTB.Text.Length < 6)
            //        {
            //            MessageBox.Show("Слишком короткий пароль.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            //            return;
            //        }
            //        else
            //        {
            //            dog.Password = PasswordTB.Text.Trim();
            //        }

            //        if (PhoneTB.Text.Length < 16)
            //        {
            //            if (PhoneTB.Text.Length < 10)
            //            {
            //                MessageBox.Show("Номер телефона должен содержать 11 цифр.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            //                return;
            //            }
            //            else
            //            {
            //                dog.Phone = PhoneTB.Text;
            //            }
            //        }

            //        if (PassportTB.Text.Length < 10)
            //        {
            //            MessageBox.Show("Паспортные данные должны содержать 10 цифр.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            //            return;
            //        }
            //        else
            //        {
            //            dog.PassportDetails = PassportTB.Text;
            //        }

            //        var a = PositionCB.SelectedItem as Position;
            //        dog.ID_Position = a.ID;

            //        var b = GenderCB.SelectedItem as Gender;
            //        dog.ID_Gender = b.ID;

            //        DBConnection.massageSalon.dog.Add(dog);
            //        DBConnection.massageSalon.SaveChanges();
            //        Close();
            //    }
            //}
            //catch
            //{

            //}

        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainAdminPage());
        }
    }
}
