using StrayDogs.DB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для AddAppointmenDoctorPage.xaml
    /// </summary>
    public partial class AddAppointmenDoctorPage : Page
    {
        public static Appointments newAppointment = new Appointments();
        public AddAppointmenDoctorPage()
        {
            InitializeComponent();
            var dogs = DBConnection.stray_DogsEntities.Dog.Where(x => x.IsDie == false && x.IsGive == false).ToList();
            DogCB.ItemsSource = dogs;
            var doctors = DBConnection.stray_DogsEntities.Employee.Where(x => x.IdPost == 2).ToList();
            VrachCB.ItemsSource = doctors;
            var status = DBConnection.stray_DogsEntities.StatusAppointment.ToList();
            StatusCB.ItemsSource = status;

            DateDayDP.DisplayDateStart = DateTime.Now;
            this.DataContext = this;
        }

        private DateTime? lastSelectedTime = null; // Храним предыдущее значение времени
        private bool isSelectingTime = false; // Флаг для отслеживания выбора времени

        private void TimeDP_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            // Если дата не выбрана, сбрасываем время и показываем сообщение
            if (DateDayDP.SelectedDate == null)
            {
                MessageBox.Show("Сначала выберите дату!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                TimeDP.SelectedTime = null;
                return;
            }

            // Если время не выбрано, ничего не делаем
            if (!TimeDP.SelectedTime.HasValue)
            {
                return;
            }

            DateTime selectedTime = TimeDP.SelectedTime.Value;
            DateTime minTime = DateTime.Today.AddHours(10);
            DateTime maxTime = DateTime.Today.AddHours(19).AddMinutes(30);

            // Проверяем, что выбранное время в допустимом диапазоне
            if (selectedTime < minTime || selectedTime > maxTime)
            {
                MessageBox.Show("Выберите время с 10:00 до 19:30!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                TimeDP.SelectedTime = null;
                return;
            }

            // Если пользователь только выбирает часы, ждем выбора минут
            if (!isSelectingTime)
            {
                isSelectingTime = true; // Устанавливаем флаг
                lastSelectedTime = selectedTime;
                return;
            }

            isSelectingTime = false; // Сбрасываем флаг после полного выбора времени

            var a = VrachCB.SelectedItem as Employee;
            var b = DogCB.SelectedItem as Dog;

            DateTime selectedDate = DateDayDP.SelectedDate.Value; // Здесь уже точно не null
            TimeSpan selectedTimeSpan = TimeDP.SelectedTime.Value.TimeOfDay;

            DateTime fullDateTime = selectedDate.Date.Add(selectedTimeSpan);
            DateTime minAllowedTime = fullDateTime.AddMinutes(-30);
            DateTime maxAllowedTime = fullDateTime.AddMinutes(30);

            var selectedVrach = DBConnection.stray_DogsEntities.Appointments
                .FirstOrDefault(x => x.IdDoctor == a.Id &&
                                     x.Date >= minAllowedTime &&
                                     x.Date <= maxAllowedTime);

            if (selectedVrach != null)
            {
                MessageBox.Show("Выбранное время занято!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                TimeDP.SelectedTime = null;
                return;
            }

            var selectedDog = DBConnection.stray_DogsEntities.Appointments
                .FirstOrDefault(x => x.IdDog == b.Id &&
                                     x.Date >= minAllowedTime &&
                                     x.Date <= maxAllowedTime);

            if (selectedDog != null)
            {
                MessageBox.Show("Собака уже записана на другой прием!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                TimeDP.SelectedTime = null;
                return;
            }
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var a = VrachCB.SelectedItem as Employee;
                var b = DogCB.SelectedItem as Dog;
                var c = StatusCB.SelectedItem as StatusAppointment;
                DateTime selectedDate = DateDayDP.SelectedDate ?? DateTime.MinValue;
                //TimeSpan selectedTimeSpan = TimeDP.SelectedTime.Value.TimeOfDay;
                DateTime fullDateTime = selectedDate.Date.Add(TimeDP.SelectedTime.Value.TimeOfDay);

                if (a == null || b == null || c == null ||
                    fullDateTime == null)
                {
                    MessageBox.Show("Заполните все поля!");
                    return;
                }
                else
                {
                    newAppointment.IdDoctor = a.Id;
                    newAppointment.IdDog = b.Id;
                    newAppointment.IdStatus = c.Id;
                    newAppointment.Date = fullDateTime;
                    newAppointment.IdStatusPriem = 2;
                    DBConnection.stray_DogsEntities.Appointments.Add(newAppointment);
                    DBConnection.stray_DogsEntities.SaveChanges();
                    MessageBox.Show("Прием успешно сохранен!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new AllAppointmentsAdminPage());
                }
            }
            catch 
            {
                MessageBox.Show("Ошибка сохранения приема!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
