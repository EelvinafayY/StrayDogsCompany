using StrayDogs.DB;
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
        public static List<Aviary> aviarys { get; set; }
        public static List<TypeAviary> typeAviaries { get; set; }
        public static List<Employee> employees { get; set; }
        public static List<Dog> dogs { get; set; }

        public MainAdminPage()
        {
            InitializeComponent();
            aviarys = DBConnection.stray_Dogs.Aviary.ToList();
            typeAviaries = DBConnection.stray_Dogs.TypeAviary.ToList();
            employees = DBConnection.stray_Dogs.Employee.ToList();
            dogs = DBConnection.stray_Dogs.Dog.ToList();
            Refresh();

        }


        public void Refresh()
        {
            VolierLV.ItemsSource = aviarys;
            WorkersLV.ItemsSource = employees;
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
            if (VolierLV.Items.Count > 0)
            {
                var index = VolierLV.SelectedIndex;
                if (index > 0)
                {
                    VolierLV.SelectedIndex = index - 1;
                    VolierLV.ScrollIntoView(VolierLV.SelectedItem);
                }
            }
        }

        private void ScrollDown_Click(object sender, RoutedEventArgs e)
        {
            if (VolierLV.Items.Count > 0)
            {
                var index = VolierLV.SelectedIndex;
                if (index < VolierLV.Items.Count - 1)
                {
                    VolierLV.SelectedIndex = index + 1;
                    VolierLV.ScrollIntoView(VolierLV.SelectedItem);
                }
            }
        }

        private void AddAviaryBT_Click(object sender, RoutedEventArgs e)
        {
            GuestApplicationWindow guestApplicationWindow = new GuestApplicationWindow();
            guestApplicationWindow.ShowDialog();
        }

        private void BeginBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BeginPage());
        }

        private void AddDogBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddDogPage());
        }

        private void AddEmployeeBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEmployeePage());
        }
    }
}
