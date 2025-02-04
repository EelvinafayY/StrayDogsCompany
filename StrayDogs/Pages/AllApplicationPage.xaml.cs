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
    /// Логика взаимодействия для AllApplicationPage.xaml
    /// </summary>
    public partial class AllApplicationPage : Page
    {
        public static List<Aplication> aplications { get; set; }
        public static List<ApplicationStatus> applicationStatuses { get; set; }
        public static List<Dog> dogs { get; set; }
        public AllApplicationPage()
        {
            InitializeComponent();

            applicationStatuses = DBConnection.stray_Dogs.ApplicationStatus.ToList();

            aplications = DBConnection.stray_Dogs.Aplication.ToList();

            foreach (var application in aplications)
            {
                application.ApplicationStatus = applicationStatuses.FirstOrDefault(s => s.IDApplicationStatus == application.IDStatusAplication);
            }

            dogs = DBConnection.stray_Dogs.Dog.ToList();

            this.DataContext = this;
        }

        private void StatusCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем текущий ComboBox
            ComboBox comboBox = sender as ComboBox;

            // Проверяем, что выбранное значение корректно
            if (comboBox?.SelectedValue is int newStatusId)
            {
                // Получаем заявку, чей статус изменился
                Aplication selectedApplication = ((FrameworkElement)sender).DataContext as Aplication;

                if (selectedApplication != null)
                {
                    // Обновляем ID статуса заявки
                    selectedApplication.IDStatusAplication = newStatusId;

                    // Обновляем объект статуса
                    selectedApplication.ApplicationStatus = applicationStatuses
                        .FirstOrDefault(s => s.IDApplicationStatus == newStatusId);

                    // Сохраняем изменения в базе
                    DBConnection.stray_Dogs.SaveChanges();
                }
            }
        }


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl && tabControl.SelectedItem is TabItem selectedTab)
            {

                Refresh();
                switch (selectedTab.Header.ToString()) // Используем Header вместо Name
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

        private void ApplicatonLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        public void Refresh()
        {
            aplications = DBConnection.stray_Dogs.Aplication.ToList();
            ApplicatonLv.ItemsSource = new List<Aplication>(DBConnection.stray_Dogs.Aplication.ToList());
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Aplication aplication)
            {
                DBConnection.stray_Dogs.Aplication.Remove(aplication);
                DBConnection.stray_Dogs.SaveChanges();

                Refresh();
            }
        }
    }
}
