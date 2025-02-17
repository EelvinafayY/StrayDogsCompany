using DocumentFormat.OpenXml.Spreadsheet;
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
    public partial class MainAdminPage : System.Windows.Controls.Page
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

            StatusCB.SelectedIndex = 0;
            typeAviaries.Insert(0, new TypeAviary { Name = "Показать все" });
            TypeCB.SelectedIndex = 0;

            this.DataContext = this;
            Refresh();
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void ScrollUp_Click(object sender, RoutedEventArgs e)
        {
            if (VoliersLV.Items.Count > 0)
            {
                var index = VoliersLV.SelectedIndex;
                if (index > 0)
                {
                    VoliersLV.SelectedIndex = index - 1;
                    VoliersLV.ScrollIntoView(VoliersLV.SelectedItem);
                }
            }
        }

        private void ScrollDown_Click(object sender, RoutedEventArgs e)
        {
            if (VoliersLV.Items.Count > 0)
            {
                var index = VoliersLV.SelectedIndex;
                if (index < VoliersLV.Items.Count - 1)
                {
                    VoliersLV.SelectedIndex = index + 1;
                    VoliersLV.ScrollIntoView(VoliersLV.SelectedItem);
                }
            }
        }

        private void AddAviaryBT_Click(object sender, RoutedEventArgs e)
        {
            int aviaryCount = DBConnection.stray_DogsEntities.Aviary.Count();
            if (aviaryCount >= 250)
            {
                MessageBox.Show("Невозможно добавить больше 250 вольеров.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                NavigationService.Navigate(new AddAviaryPage());
            }
        }


        private void AddDogBTN_Click(object sender, RoutedEventArgs e)
        {
            int dogCount = DBConnection.stray_DogsEntities.Dog.Count();
            if (dogCount >= 1999)
            {
                MessageBox.Show("Невозможно добавить больше 1 999 собак.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                NavigationService.Navigate(new AddDogPage());
            }

        }

        private void AddEmployeeBT_Click(object sender, RoutedEventArgs e)
        {
            int emplCount = DBConnection.stray_DogsEntities.Employee.Count();
            if (emplCount >= 100)
            {
                MessageBox.Show("Невозможно добавить больше 100 сотрудников.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {            
                NavigationService.Navigate(new AddEmployeePage());
            }

        }

        private void EditBTN_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Хотите отредактировать данные о сотруднике?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            var worker = (sender as Button).DataContext as Employee;
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new Stray_DogsEntities())
                {
                    var serviceToEdit = DBConnection.stray_DogsEntities.Employee.Find(worker.Id);

                    if (serviceToEdit != null)
                    {
                        NavigationService.Navigate(new EditEmployeePage(worker));
                        Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Сотрудник не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        Refresh();
                    }
                }
                Refresh();
            }
            else if (result == MessageBoxResult.No) { Refresh(); }
        }

        private void DeleteBTN_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            var worker = (sender as Button).DataContext as Employee;
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new Stray_DogsEntities())
                {
                    var serviceToDelete = DBConnection.stray_DogsEntities.Employee.Find(worker.Id);

                    if (serviceToDelete != null)
                    {
                        DBConnection.stray_DogsEntities.Employee.Remove(worker);
                        DBConnection.stray_DogsEntities.SaveChanges();

                        MessageBox.Show("Сотрудник удален.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Сотрудник не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        Refresh();
                    }
                }
                Refresh();
            }
            else if (result == MessageBoxResult.No) { Refresh(); }
        }



        private void TypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void StatusCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            var filterWorker = DBConnection.stray_DogsEntities.Employee.ToList();
            var filterVoliers = DBConnection.stray_DogsEntities.Aviary.ToList();

            if (SearchTB.Text.Length > 0)
            {
                filterWorker = filterWorker
                    .Where(i => i.Surname.ToLower().Contains(SearchTB.Text.Trim().ToLower())
                             || i.Name.ToLower().Contains(SearchTB.Text.Trim().ToLower())
                             || i.Patronymic.ToLower().Contains(SearchTB.Text.Trim().ToLower()))
                    .ToList();

                filterVoliers = filterVoliers
                                    .Where(i => i.TypeAviary.Name.ToLower().Contains(SearchTB.Text.Trim().ToLower())
                                     || i.Number.ToLower().Contains(SearchTB.Text.Trim().ToLower()))
                                    .ToList();
            }

            var typeAviary = TypeCB.SelectedItem as TypeAviary;
            if (TypeCB.SelectedIndex > 0)
                filterVoliers = filterVoliers.Where(x => x.IdType == typeAviary.Id).ToList();

            var statusAviary = StatusCB.SelectedItem as ComboBoxItem;
            if (StatusCB.SelectedIndex > 0)
            {
                string selectedStatus = statusAviary.Content.ToString();
                filterVoliers = filterVoliers.Where(x => x.Status == selectedStatus).ToList();
            }

            VoliersLV.ItemsSource = filterVoliers;
            WorkersLV.ItemsSource = filterWorker;

            WorkersLV.LayoutUpdated += WorkersLV_LayoutUpdated;
        }
        private void WorkersLV_LayoutUpdated(object sender, EventArgs e)
        {
            foreach (var item in WorkersLV.Items)
            {
                ListViewItem listViewItem = (ListViewItem)WorkersLV.ItemContainerGenerator.ContainerFromItem(item);
                if (listViewItem != null)
                {
                    Button deleteButton = FindChild<Button>(listViewItem, "DeleteBTN");
                    if (deleteButton != null)
                    {
                        var employee = item as Employee;
                        if (employee != null && employee.Id == loggedEmployee.Id)
                        {
                            deleteButton.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            deleteButton.Visibility = Visibility.Visible;
                        }
                    }
                }
            }

            WorkersLV.LayoutUpdated -= WorkersLV_LayoutUpdated;
        }
        private T FindChild<T>(DependencyObject parent, string childName) where T : FrameworkElement
        {
            if (parent == null) return null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T frameworkElement && frameworkElement.Name == childName)
                    return frameworkElement;

                T foundChild = FindChild<T>(child, childName);
                if (foundChild != null)
                    return foundChild;
            }

            return null;
        }
    }
}
