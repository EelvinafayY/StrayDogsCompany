using StrayDogs.DB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
    /// Логика взаимодействия для GuestHomePage.xaml
    /// </summary>
    public partial class GuestHomePage : Page
    {
        public GuestHomePage()
        {
            InitializeComponent();
            var dogs = DB.DBConnection.stray_DogsEntities.Dog.Where(x => x.IsGive == false && x.IsDie == false).ToList();
            DogsLv.ItemsSource = dogs;
            this.DataContext = this;
        }

        private void InfoDogPage_Click(object sender, RoutedEventArgs e)
        {
            var dogs = (sender as Button).DataContext as Dog;
            if (dogs != null)
            {
                NavigationService.Navigate(new GuestDogPage(dogs));
            }
        }


        private void ExitBT_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите выйти из системы?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                //ПЕРЕЗАПУСК ПРОГРАММЫ
                string exePath = Process.GetCurrentProcess().MainModule.FileName;
                Process.Start(exePath);
                Application.Current.Shutdown();
            }
        }
    }
}
