using Microsoft.Win32;
using StrayDogs.DB;
using StrayDogs.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace StrayDogs.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditDogPage.xaml
    /// </summary>
    public partial class EditDogPage : Page
    {
        // Статические свойства для списков
        public static List<Gender> genders { get; set; }
        public static List<Aviary> aviaries { get; set; }
        public static List<TypeAviary> typeAviaries { get; set; }

        Dog contextdog;

        public EditDogPage(Dog dog)
        {
            InitializeComponent();
            contextdog = dog;

            genders = DBConnection.stray_DogsEntities.Gender.ToList();
            aviaries = DBConnection.stray_DogsEntities.Aviary.ToList();
            typeAviaries = DBConnection.stray_DogsEntities.TypeAviary.ToList();

            LoadAviaryOptions();

            IsGiveCB.IsChecked = contextdog.IsGive;
            IsDiedCHB.IsChecked = contextdog.IsDie;
            DataContext = contextdog;
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите отменить все изменения?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                NavigationService.GoBack();
            }
        }

        private void LoadAviaryOptions()
        {
            AviaryCB.ItemsSource = null;
            var query = DBConnection.stray_DogsEntities.Aviary.AsQueryable();

            if (contextdog.Gender.Name == "Самец")
            {
                query = query.Where(i => i.TypeAviary.Name == "Обычный");
            }

            var availableAviaries = query.ToList();
            AviaryCB.ItemsSource = availableAviaries;

            if (contextdog.IdAviary.HasValue)
            {
                var currentAviary = availableAviaries.FirstOrDefault(a => a.Id == contextdog.IdAviary.Value);
                if (currentAviary != null)
                {
                    AviaryCB.SelectedItem = currentAviary;
                }
            }
        }

        private void PhotoEditBTN_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image Files|*.png;*.jpeg;*.jpg"
            };

            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                contextdog.Photo = File.ReadAllBytes(openFileDialog.FileName);
                PhotoDog.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void OKBTN_Click(object sender, RoutedEventArgs e)
        {
            var selectedAviary = AviaryCB.SelectedItem as Aviary;
            if (selectedAviary != null)
            {
                contextdog.IdAviary = selectedAviary.Id;
            }

            try
            {
                DBConnection.stray_DogsEntities.SaveChanges();

                MessageBoxResult result = MessageBox.Show($"Данные о собаке были изменены!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);

                if (result == MessageBoxResult.OK)
                {
                    NavigationService.Navigate(new AllDogsPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении изменений: " + ex.Message);
            }
        }

        private void AllAppoitmentsDogBTN_Click(object sender, RoutedEventArgs e)
        {
            AllAppointmentsDogWindow allAppointmentsDog = new AllAppointmentsDogWindow(contextdog);
            allAppointmentsDog.ShowDialog();
        }
    }
}
