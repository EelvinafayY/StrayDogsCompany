using StrayDogs.DB;
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
    /// Логика взаимодействия для EditAppointmentPage.xaml
    /// </summary>
    public partial class EditAppointmentPage : Page
    {
        Appointments contextAppointment;

        public bool IsClearButtonVisible { get; set; }
        public EditAppointmentPage(Appointments appointments)
        {
            InitializeComponent();
            contextAppointment = appointments;
            VrachTB.Text = appointments.Employee.FullName;
            DataContext = contextAppointment = appointments;
            VisibilityAdmin();
            VisibilityUser();

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if(DBConnection.logginedEmployee.IdPost == 2)
            {
                if(contextAppointment.IdStatusPriem == 2)
                {
                    MessageBoxResult result = MessageBox.Show($"Вы действительно хотите отменить все изменения?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        NavigationService.GoBack();
                    }
                }
                else
                {
                    NavigationService.GoBack();
                }
            }
            else
            {
                NavigationService.GoBack();
            }



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (contextAppointment.IdStatusPriem == 1)
            {
                MessageBox.Show("Данный прием уже был завершен, редактирование невозможно!");
                return;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(CommentTb.Text) != true || string.IsNullOrWhiteSpace(DieseTb.Text) != true)
                {
                    if (contextAppointment.Date > DateTime.Now)
                    {
                        MessageBox.Show("Вы не можете завершить данный прием, так как время его начала еще не наступило!");
                        return;
                    }
                    contextAppointment.IdStatusPriem = 1;
                    contextAppointment.Comment = CommentTb.Text;
                    contextAppointment.Disease = DieseTb.Text;
                    if (DieCheck.IsChecked == true)
                    {
                        MessageBoxResult result = MessageBox.Show($"Вы действительно хотите сделать отметку о том, что собака мертва?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            contextAppointment.Dog.IsDie = true;
                            MessageBox.Show("С этого момента собака считается мертвой(");
                            var otherDogAppointment = DBConnection.stray_DogsEntities.Appointments
                                    .Where(a => a.IdDog == contextAppointment.IdDog &&
                                                a.IdStatusPriem == 2 &&
                                                a.Id != contextAppointment.Id)
                                    .ToList();

                            foreach (var application in otherDogAppointment)
                            {
                                application.IdStatusPriem = 3;
                            }
                        }
                        if (result == MessageBoxResult.No)
                        {
                            DieCheck.IsChecked = false;
                            contextAppointment.Dog.IsDie = false;
                        }
                    }

                    DBConnection.stray_DogsEntities.SaveChanges();

                    MessageBox.Show($"Прием завершен!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigationService.Navigate(new MainDoctorPage());
                }
                else
                {
                    MessageBox.Show("Заполните все необходимые поля!");
                    return;
                }
            }
        }


        private void DieCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            if (contextAppointment.Dog.IsDie == true)
            {
                DieCheck.IsChecked = true;
            }
            else
            {
                DieCheck.IsChecked = false;
            }

        }

        private void DieCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (contextAppointment.Dog.IsDie == false)
            {
                DieCheck.IsChecked = true;
            }

        }

        public void VisibilityUser()
        {
            if (contextAppointment.IdStatusPriem == 1 || contextAppointment.IdStatusPriem == 3)
            {
                OverBtn.Visibility = Visibility.Collapsed;
                DieseTb.IsReadOnly = true;
                CommentTb.IsReadOnly = true;
                DieCheck.Visibility = Visibility.Collapsed;
                if (contextAppointment.Dog.IsDie == false)
                {
                    DieTB.Visibility = Visibility.Collapsed;
                }
            }

        }

        public void VisibilityAdmin()
        {
            if(DBConnection.logginedEmployee.IdPost == 1)
            {
                OverBtn.Visibility = Visibility.Collapsed;
                DieseTb.IsReadOnly = true;
                CommentTb.IsReadOnly = true;
                DieCheck.IsEnabled = false;
                IsClearButtonVisible = DBConnection.logginedEmployee.IdPost != 1;
            }
        }
    }
}
