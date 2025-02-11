using StrayDogs.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddDogPage.xaml
    /// </summary>
    public partial class AddDogPage : Page
    {
        public static Dog dog = new Dog();
        public static List<Gender> genders { get; set; }
        public static List<Aviary> aviaries { get; set; }

        public AddDogPage()
        {
            InitializeComponent();
            genders = DBConnection.stray_DogsEntities.Gender.ToList();
            aviaries = DBConnection.stray_DogsEntities.Aviary.ToList();
            this.DataContext = this;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Используем регулярное выражение для проверки, является ли введенный символ цифрой
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AddBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NumberTB.Text) || string.IsNullOrWhiteSpace(HeightTB.Text) ||
                       string.IsNullOrWhiteSpace(WeightTB.Text) || string.IsNullOrWhiteSpace(AgeTB.Text) ||
                       string.IsNullOrWhiteSpace(DescriptionTB.Text) || GenderCB.SelectedItem == null || VolierCB.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все поля.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    dog.OrdinalNumber = NumberTB.Text.Trim();
                    dog.Height = int.Parse(HeightTB.Text.Trim());
                    dog.Weight = int.Parse(WeightTB.Text.Trim());
                    dog.Age = int.Parse(AgeTB.Text.Trim()); ;

                    string number = NumberTB.Text;
                    bool numberExists = DBConnection.stray_DogsEntities.Dog.Any(w => w.OrdinalNumber == number);

                    if (numberExists)
                    {
                        MessageBox.Show("Этот логин уже занят. Выберите другой.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        if (NumberTB.Text.Length > 10)
                        {
                            MessageBox.Show("Слишком длинный номер.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        else if (NumberTB.Text.Length < 5)
                        {
                            MessageBox.Show("Слишком короткий номер.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        else
                        {
                            dog.OrdinalNumber = NumberTB.Text.Trim();
                        }
                    }

                    if ()
                    {

                    }
                    else
                    {
                        dog.Height = int.Parse(HeightTB.Text.Trim());
                    }

                    var a = VolierCB.SelectedItem as Aviary;
                    dog.IdAviary = a.Id;

                    var b = GenderCB.SelectedItem as Gender;
                    dog.IdGender = b.Id;

                    DBConnection.stray_DogsEntities.Dog.Add(dog);
                    DBConnection.stray_DogsEntities.SaveChanges();
                }
            }
            catch
            {

            }

        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainAdminPage());
        }
    }
}
