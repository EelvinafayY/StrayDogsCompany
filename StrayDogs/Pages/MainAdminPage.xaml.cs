using StrayDogs.Windows;
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
    /// Логика взаимодействия для MainAdminPage.xaml
    /// </summary>
    public partial class MainAdminPage : Page
    {
        public MainAdminPage()
        {
            InitializeComponent();

            List<string> list = new List<string>(){"Первый", "Second", "Third", "Fourht", "Five"};
            VolierLv.ItemsSource = list;    

            WorkersLv.ItemsSource = list;   

        }

        private void TabControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void probaI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //NavigationService.Navigate(new ProbaPage());
        }

        private void Lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ScrollUp_Click(object sender, RoutedEventArgs e)
        {
            if (VolierLv.Items.Count > 0)
            {
                var index = VolierLv.SelectedIndex;
                if (index > 0)
                {
                    VolierLv.SelectedIndex = index - 1;
                    VolierLv.ScrollIntoView(VolierLv.SelectedItem);
                }
            }
        }

        private void ScrollDown_Click(object sender, RoutedEventArgs e)
        {
            if (VolierLv.Items.Count > 0)
            {
                var index = VolierLv.SelectedIndex;
                if (index < VolierLv.Items.Count - 1)
                {
                    VolierLv.SelectedIndex = index + 1;
                    VolierLv.ScrollIntoView(VolierLv.SelectedItem);
                }
            }
        }

        private void AddAviaryBT_Click(object sender, RoutedEventArgs e)
        {
            GuestApplicationWindow guestApplicationWindow = new GuestApplicationWindow();
            guestApplicationWindow.ShowDialog();
        }
    }
}
