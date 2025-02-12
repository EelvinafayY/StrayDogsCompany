using StrayDogs.DB;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для EditAppointmentPage.xaml
    /// </summary>
    public partial class EditAppointmentPage : Page
    {

        Appointments contextAppointment;
        public EditAppointmentPage(Appointments appointments)
        {
            InitializeComponent();

            contextAppointment = appointments;
            DataContext = contextAppointment = appointments;


        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainDoctorPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(contextAppointment.IdStatusPriem == 1)
            {
                MessageBox.Show("Данный прием уже был завершен, редактирование невозможно!");
                return;
            }
            else
            {
                if(string.IsNullOrWhiteSpace(CommentTb.Text) != true || string.IsNullOrWhiteSpace(DieseTb.Text) != true)
                {
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
                        }
                        if (result == MessageBoxResult.No)
                        {
                            DieCheck.IsChecked = false;
                            contextAppointment.Dog.IsDie = false;
                        }
                    }
                    
                    DBConnection.stray_DogsEntities.SaveChanges();
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
            DieCheck.IsChecked = false;
            contextAppointment.Dog.IsDie = false;
        }

        private void DieCheck_Checked(object sender, RoutedEventArgs e)
        {
            DieCheck.IsChecked = true;
            contextAppointment.Dog.IsDie = true;

        }
    }
}
