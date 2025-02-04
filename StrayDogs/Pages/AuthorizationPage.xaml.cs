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
using StrayDogs.DB;

namespace StrayDogs.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public static List<Employee> employees { get; set; }

        public static List<Post> posts { get; set; }

        public AuthorizationPage()
        {
            InitializeComponent();


        }

        private void EnterBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = LoginTB.Text.Trim();
                string password = PasswordTB.Text.Trim();

                employees = DBConnection.stray_DogsEntities.Employee.ToList();
                posts = DBConnection.stray_DogsEntities.Post.ToList();

                var currentEmployee = employees.FirstOrDefault(i => i.Login.Trim() == login && i.Password.Trim() == password);
                DBConnection.logginedEmployee = currentEmployee;

                if (currentEmployee != null && currentEmployee.Post.Name == "Администратор")
                {
                    NavigationService.Navigate(new AdminMenuPage());
                }
                else if (currentEmployee != null && currentEmployee.Post.Name == "Врач")
                {
                    NavigationService.Navigate(new DoctorMenuPage());
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Возникла ошибка!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegistrationBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}
