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
    /// Логика взаимодействия для BeginPage.xaml
    /// </summary>
    public partial class BeginPage : Page
    {
        public BeginPage()
        {
            InitializeComponent();
        }

        private void EmloyeeAuthBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());
        }

        private void GuestAuthBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GuestHomePage());
        }
    }
}
