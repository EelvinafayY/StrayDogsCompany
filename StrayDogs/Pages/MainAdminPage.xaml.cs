using StrayDogs.DB;
using StrayDogs.Windows;
using System;
using System.Collections.Generic;
using System.IO;
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
        Employee loggedEmployee;
        public static List<Aviary> aviarys { get; set; }
        public static List<TypeAviary> typeAviaries { get; set; }
        public static List<Employee> employees { get; set; }
        public static List<Dog> dogs { get; set; }

        public MainAdminPage()
        {
            InitializeComponent();
            loggedEmployee = DBConnection.logginedEmployee;

            if (loggedEmployee.Photo != null)
            {
                using (var stream = new MemoryStream(loggedEmployee.Photo))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    WorkerPhoto.Source = bitmap;
                }
            }

            FioTB.Text = loggedEmployee.Surname + " " + loggedEmployee.Name + " " + loggedEmployee.Patronymic;
            aviarys = DBConnection.stray_DogsEntities.Aviary.ToList();
            typeAviaries = DBConnection.stray_DogsEntities.TypeAviary.ToList();
            employees = DBConnection.stray_DogsEntities.Employee.ToList();
            dogs = DBConnection.stray_DogsEntities.Dog.ToList();
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

        private void AddDogBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddDogPage());
        }

        private void AddEmployeeBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEmployeePage());
        }

        private void EditBTN_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Хотите отредактировать данные о сотруднике?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            var worker = (sender as Button).DataContext as Employee;
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new Stray_DogsEntities1())
                {
                    var serviceToDelete = DBConnection.stray_DogsEntities.Employee.Find(worker.Id);

                    if (serviceToDelete != null)
                    {
                        NavigationService.Navigate(new EditEmployeePage(worker));
                        DBConnection.stray_DogsEntities.SaveChanges();

                        MessageBox.Show("Данные сохранены.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                        Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Сотрудник не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                        Refresh();
                    }
                }
                Refresh();
            }
            else if (result == MessageBoxResult.No) { Refresh(); }
        }
        //
        private void DeleteBTN_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            var worker = (sender as Button).DataContext as Employee;
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new Stray_DogsEntities1())
                {
                    // Находим клиента по его ID
                    var serviceToDelete = DBConnection.stray_DogsEntities.Employee.Find(worker.Id);

                    if (serviceToDelete != null)
                    {
                        // Удаляем клиента из контекста
                        DBConnection.stray_DogsEntities.Employee.Remove(worker);
                        DBConnection.stray_DogsEntities.SaveChanges();

                        MessageBox.Show("Сотрудник удален.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                        Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Сотрудник не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                        Refresh();
                    }
                }
                Refresh();
            }
            else if (result == MessageBoxResult.No) { Refresh(); }
        }

        private void BeginBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());
            NavigationService.Navigate(new BeginPage());
        }
    }
}
