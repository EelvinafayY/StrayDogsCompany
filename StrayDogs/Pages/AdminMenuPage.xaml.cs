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
    /// Логика взаимодействия для AdminMenuPage.xaml
    /// </summary>
    public partial class AdminMenuPage : Page
    {
        public AdminMenuPage()
        {
            InitializeComponent();
            ContentDoctorFrame.NavigationService.Navigate(new MainAdminPage());
        }

        private void AppointmentsTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void EmployeeTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        private void HomeTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ContentDoctorFrame.NavigationService.Navigate(new MainAdminPage());
        }

        private void DogTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ContentDoctorFrame.NavigationService.Navigate(new AllDogsPage());
        }

        private void AccountTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
