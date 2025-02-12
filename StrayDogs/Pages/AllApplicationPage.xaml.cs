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
using StrayDogs.DB;

namespace StrayDogs.Pages
{
    /// <summary>
    /// Логика взаимодействия для AllApplicationPage.xaml
    /// </summary>
    public partial class AllApplicationPage : Page
    {
        private bool isManualChange = false;
        public static List<Aplication> aplications { get; set; }
        public static List<ApplicationStatus> applicationStatuses { get; set; }
        public static List<Dog> dogs { get; set; }
        public static List<Employee> employees { get; set; }
        Employee loggedEmployee;
        public AllApplicationPage()
        {
            InitializeComponent();
            applicationStatuses = DBConnection.stray_DogsEntities.ApplicationStatus.ToList();
            aplications = DBConnection.stray_DogsEntities.Aplication.ToList();
            foreach (var application in aplications)
            {
                application.ApplicationStatus = applicationStatuses.FirstOrDefault(s => s.IDApplicationStatus == application.IDStatusAplication);
            }
            dogs = DBConnection.stray_DogsEntities.Dog.ToList();
            employees = DBConnection.stray_DogsEntities.Employee.ToList();
            loggedEmployee = DBConnection.logginedEmployee;
            FioTB.Text = loggedEmployee.Surname + " " + loggedEmployee.Name + " " + loggedEmployee.Patronymic;
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
            this.DataContext = this;
        }
        private bool isUpdating = false; // Флаг предотвращает зацикливание

        private void StatusCb_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox?.DataContext is Aplication selectedApplication)
            {
                isUpdating = true;
                comboBox.SelectedValue = selectedApplication.IDStatusAplication;
                isUpdating = false;
            }
        }

        private void StatusCb_DropDownClosed(object sender, EventArgs e)
        {
            if (isUpdating) return;

            ComboBox comboBox = sender as ComboBox;
            if (comboBox?.SelectedValue is int newStatusId && comboBox.DataContext is Aplication selectedApplication)
            {
                int? oldStatus = DBConnection.stray_DogsEntities.Aplication
                    .Where(a => a.IDApplication == selectedApplication.IDApplication)
                    .Select(a => a.IDStatusAplication)
                    .FirstOrDefault();

                if (oldStatus != 3)
                {
                    MessageBox.Show("Смена статуса невозможна, решение уже принято!");
                    ResetComboBox(comboBox, oldStatus);
                    return;
                }

                var selectedDog = DBConnection.stray_DogsEntities.Dog
                    .FirstOrDefault(x => x.Id == selectedApplication.IDDog && x.IsGive == false);

                if (selectedDog != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Вы действительно хотите сменить статус заявки?\nИзменить статус в дальнейшем будет невозможно.",
                        "Подтверждение",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning
                    );

                    if (result == MessageBoxResult.No)
                    {
                        ResetComboBox(comboBox, oldStatus);
                        return;
                    }

                    if (result == MessageBoxResult.Yes)
                    {
                        if (newStatusId == 1)
                        {
                            var selectedDogg = DBConnection.stray_DogsEntities.Dog
                                .FirstOrDefault(x => x.Id == selectedApplication.IDDog);

                            if (selectedDogg != null)
                            {
                                selectedDogg.IsGive = true;
                                selectedDogg.NumberPhoneHost = selectedApplication.Phone;
                                selectedApplication.IDStatusAplication = 1;

                                var otherPendingApplications = DBConnection.stray_DogsEntities.Aplication
                                    .Where(a => a.IDDog == selectedApplication.IDDog &&
                                                a.IDStatusAplication == 3 &&
                                                a.IDApplication != selectedApplication.IDApplication)
                                    .ToList();

                                foreach (var application in otherPendingApplications)
                                {
                                    application.IDStatusAplication = 2; // Отказано
                                }
                            }
                        }

                        if (newStatusId == 2)
                        {
                            selectedApplication.IDStatusAplication = 2;
                        }

                        DBConnection.stray_DogsEntities.SaveChanges();
                        NavigationService.Navigate(new AllApplicationPage());
                    }
                }
            }
        }

        private void ResetComboBox(ComboBox comboBox, int? oldValue)
        {
            isUpdating = true;
            comboBox.SelectedValue = oldValue ?? -1;
            isUpdating = false;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl && tabControl.SelectedItem is TabItem selectedTab)
            {
                Refresh();

                switch (selectedTab.Header.ToString())
                {
                    case "Все заявки":
                        ApplicatonLv.ItemsSource = aplications;
                        break;

                    case "Одобрены":
                        ApplicatonLv.ItemsSource = aplications.Where(i => i.IDStatusAplication == 1).ToList();
                        break;

                    case "Отклонены":
                        ApplicatonLv.ItemsSource = aplications.Where(i => i.IDStatusAplication == 2).ToList();
                        break;

                    case "На рассмотрении":
                        ApplicatonLv.ItemsSource = aplications.Where(i => i.IDStatusAplication == 3).ToList();
                        break;
                }
            }
        }

        public void Refresh()
        {
            aplications = DBConnection.stray_DogsEntities.Aplication.ToList();
            ApplicatonLv.ItemsSource = new List<Aplication>(DBConnection.stray_DogsEntities.Aplication.ToList());
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Aplication aplication)
            {
                DBConnection.stray_DogsEntities.Aplication.Remove(aplication);
                DBConnection.stray_DogsEntities.SaveChanges();

                Refresh();
            }
        }

    }
}
