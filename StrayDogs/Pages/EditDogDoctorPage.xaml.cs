using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static StrayDogs.Pages.AccountPage;
using StrayDogs.DB;
using System.IO;

namespace StrayDogs.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditDogDoctorPage.xaml
    /// </summary>
    public partial class EditDogDoctorPage : Page
    {
        public string OrdinalNumberBoxText { get; set; }
        public string HeightBoxText { get; set; }
        public string WeightBoxText { get; set; }
        public string AgeBoxText { get; set; }
        public string DescriptionBoxText { get; set; }
        public string PhoneBoxText { get; set; }
        public string GenderBoxText { get; set; }
        public string AviaryBoxText { get; set; }
        public ObservableCollection<SuggestionItem> OrdinalNumberBoxSuggestions { get; set; }
        public ObservableCollection<SuggestionItem> HeightBoxSuggestions { get; set; }
        public ObservableCollection<SuggestionItem> WeightBoxSuggestions { get; set; }
        public ObservableCollection<SuggestionItem> AgeBoxSuggestions { get; set; }
        public ObservableCollection<SuggestionItem> DescriptionBoxSuggestions { get; set; }
        public ObservableCollection<SuggestionItem> PhoneBoxSuggestions { get; set; }
        public ObservableCollection<SuggestionItem> GenderBoxSuggestions { get; set; }
        public ObservableCollection<SuggestionItem> AviaryBoxSuggestions { get; set; }

        public static List<Dog> dogs {  get; set; }
        public static List<Gender> genders { get; set; }
        public static List<Aviary> aviaries { get; set; }
        Dog contextDog;
        public EditDogDoctorPage(Dog dog)
        {
            InitializeComponent();
            contextDog = dog;
            InfoView();

            this.DataContext = this;

        }

        public void InfoView()
        {

            dogs = DBConnection.stray_DogsEntities.Dog.ToList();
            genders = DBConnection.stray_DogsEntities.Gender.ToList();
            aviaries = DBConnection.stray_DogsEntities.Aviary.ToList();

            OrdinalNumberBoxText = contextDog.OrdinalNumber;
            HeightBoxText = contextDog.Height.ToString();
            WeightBoxText = contextDog.Weight.ToString();
            AgeBoxText = contextDog.Age.ToString();
            DescriptionBoxText = contextDog.Description;
            GenderBoxText = contextDog.Gender.Name;

            if(contextDog.IdAviary == null) { AviaryBoxText = " "; }
            else { AviaryBoxText = contextDog.Aviary.TypeAviary.Name;}

            
            if (contextDog.NumberPhoneHost == null) { PhoneBoxText = " "; }
            else {PhoneBoxText = contextDog.NumberPhoneHost.ToString(); }


            // Устанавливаем значения для флажков, учитывая возможное значение null
            IsDeadCKB.IsChecked = contextDog.IsDie.HasValue ? contextDog.IsDie.Value : false;
            IsGiveCKB.IsChecked = contextDog.IsGive.HasValue ? contextDog.IsGive.Value : false;


            if (contextDog.Photo != null)
            {
                using (var stream = new MemoryStream(DBConnection.logginedEmployee.Photo))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    PhotoDog.Source = bitmap;
                }
            }
        }

        private void BeginBTN_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите вернуться назад?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new AllDogsPage());
            }

        }

        private void appoitmentsBTN_Click(object sender, RoutedEventArgs e)
        {

        }

        public class SuggestionItem
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
