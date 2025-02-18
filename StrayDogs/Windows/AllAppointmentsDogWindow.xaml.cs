using StrayDogs.DB;
using StrayDogs.Pages;
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

namespace StrayDogs.Windows
{
    /// <summary>
    /// Логика взаимодействия для AllAppointmentsDogWindow.xaml
    /// </summary>
    public partial class AllAppointmentsDogWindow : Window
    {
        public static List<Appointments> appointments { get; set; }
        Dog contextdog;
        public AllAppointmentsDogWindow(Dog dog)
        {
            InitializeComponent();
            contextdog = dog;

            var appointments = DBConnection.stray_DogsEntities.Appointments.Where(x => x.IdDog == contextdog.Id).ToList();

            PriemsLv.ItemsSource = appointments;
            Refresh();
            this.DataContext = this;
        }

        private void InfoDogPageBTN_Click(object sender, RoutedEventArgs e)
        {
            var selectedAppointmentt = (sender as Button).DataContext as Appointments;

            if (selectedAppointmentt != null)
            {
                
                //NavigationService.Navigate(selectedAppointmentt));
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите прием с собакой для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Refresh()
        {
            var filtred = DBConnection.stray_DogsEntities.Appointments.Where(x => x.IdDog == contextdog.Id).ToList();

            DateTime now = DateTime.Now.Date;
            var appointments = DBConnection.stray_DogsEntities.Appointments.Where(x => x.IdDog == contextdog.Id).ToList();
            IEnumerable<Appointments> filteredAppointments = appointments;

            // Фильтрация по дате
            switch (DateSortCB.SelectedIndex)
            {
                case 0: // Сегодня
                    filtred = filtred.Where(a => a.Date.HasValue && a.Date.Value.Date == now).ToList();
                    break;
                case 1: // Вчера
                    filtred = filtred.Where(a => a.Date.HasValue && a.Date.Value.Date == now.AddDays(-1)).ToList();
                    break;
                case 2: // Последние 7 дней
                    DateTime sevenDaysAgo = now.AddDays(-7);
                    filtred = filtred.Where(a => a.Date.HasValue && a.Date.Value.Date >= sevenDaysAgo && a.Date.Value.Date <= now).ToList();
                    break;
                case 3: // Последний месяц
                    DateTime oneMonthAgo = now.AddMonths(-1);
                    filtred = filtred.Where(a => a.Date.HasValue && a.Date.Value.Date >= oneMonthAgo && a.Date.Value.Date <= now).ToList();
                    break;
                case 4: // Без фильтра по дате
                    break;
                default:
                    break;
            }


            PriemsLv.ItemsSource = filtred;
        }

        private void DateSortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }
    }
}
