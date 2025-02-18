using Microsoft.Win32;
using StrayDogs.DB;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static List<TypeAviary> typeAviaries { get; set; }
        public static List<Dog> dogs { get; set; }

        public AddDogPage()
        {
            InitializeComponent();
            genders = DBConnection.stray_DogsEntities.Gender.ToList();
            aviaries = DBConnection.stray_DogsEntities.Aviary.ToList();
            typeAviaries = DBConnection.stray_DogsEntities.TypeAviary.ToList();
            GenerateAndDisplayOrigNumber();
            this.DataContext = this;
        }

        private void GenerateAndDisplayOrigNumber()
        {
            for (int i = 1; i <= 1999; i++)
            {
                string generatedOrigNumber = $"с{(i).ToString("D5")}"; // Генерация логина типа а00001, а00002 и т.д.

                bool loginExists = DBConnection.stray_DogsEntities.Dog.Any(w => w.OrdinalNumber == generatedOrigNumber);

                if (!loginExists)
                {
                    NumberTB.Text = generatedOrigNumber; // Обновляем свойство aviary.Number
                    break;
                }
                else if (i == 1999)
                {
                    MessageBox.Show("Добавление невозможно!. Нет доступного места.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Используем регулярное выражение для проверки, является ли введенный символ цифрой
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);

            TextBox textBox = (TextBox)sender;
            string currentText = textBox.Text;
            if (currentText.Length >= 3 && !string.IsNullOrEmpty(e.Text))
            {
                e.Handled = true;
                return;
            }

            string currentText2 = AgeTB.Text;
            if (currentText2.Length >= 2 && !string.IsNullOrEmpty(e.Text))
            {
                e.Handled = true;
                return;
            }
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

                    if (int.Parse(HeightTB.Text.Trim()) < 1 || int.Parse(HeightTB.Text.Trim()) > 200)
                    {
                        MessageBox.Show("Введите нормальное значение.");
                        return;
                    }
                    else
                    {
                        dog.Height = int.Parse(HeightTB.Text.Trim());
                    }

                    if (int.Parse(WeightTB.Text.Trim()) < 1 || int.Parse(WeightTB.Text.Trim()) > 200)
                    {
                        MessageBox.Show("Введите нормальное значение.");
                        return;
                    }
                    else
                    {
                        dog.Weight = int.Parse(WeightTB.Text.Trim());
                    }

                    if (int.Parse(AgeTB.Text.Trim()) < 1 || int.Parse(AgeTB.Text.Trim()) > 40)
                    {
                        MessageBox.Show("Введите нормальное значение.");
                        return;
                    }
                    else
                    {
                        dog.Age = int.Parse(AgeTB.Text.Trim());
                    }

                    if (dog.Photo == null)
                    {
                        MessageBox.Show("Добавьте фото.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var a = VolierCB.SelectedItem as Aviary;
                    dog.IdAviary = a.Id;

                    var b = GenderCB.SelectedItem as Gender;
                    dog.IdGender = b.Id;

                    dog.Description = DescriptionTB.Text;
                    dog.IsDie = false;
                    dog.IsGive = false;

                    DBConnection.stray_DogsEntities.Dog.Add(dog);
                    DBConnection.stray_DogsEntities.SaveChanges();

                    MessageBoxResult result = MessageBox.Show($"Собака добавлена.", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (result == MessageBoxResult.OK)
                    {
                        NavigationService.Navigate(new MainAdminPage());
                    }
                    else { NavigationService.Navigate(new MainAdminPage()); }
                }
            }
            catch
            {
                MessageBox.Show("Непредвиденная ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddPhotoBTN_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg"
            };
            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                dog.Photo = File.ReadAllBytes(openFileDialog.FileName);
                dogPhoto.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }

            TextTbBTN.Text = "Изменить фото";
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите отменить все изменения?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new MainAdminPage());
            }
        }

        private void GenderCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GenderCB.SelectedItem == null)
            {
                VolierCB.IsEnabled = false;
                VolierCB.ItemsSource = null; // Очищаем список вольеров
                return;
            }

            VolierCB.IsEnabled = true;
            var query = DBConnection.stray_DogsEntities.Aviary.AsQueryable(); // Создаем базовый запрос

            // Фильтруем по типу вольера, если это необходимо
            if (GenderCB.SelectedIndex == 0) // Например, 0 - это "Обычный"
            {
                query = query.Where(i => i.TypeAviary.Name == "Обычный");
            }

            // Получаем только пустые вольеры:
            var emptyAviaries = query.Where(a => !DBConnection.stray_DogsEntities.Dog.Any(d => d.IdAviary == a.Id)).ToList();

            VolierCB.ItemsSource = emptyAviaries;
        }
    }
}