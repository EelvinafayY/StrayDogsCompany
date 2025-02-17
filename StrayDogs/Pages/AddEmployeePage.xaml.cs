using Microsoft.Win32;
using StrayDogs.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для AddEmployeePage.xaml
    /// </summary>
    public partial class AddEmployeePage : Page
    {
        public static Employee employee = new Employee();
        public static List<Post> posts { get; set; }

        public AddEmployeePage()
        {
            InitializeComponent();

            posts = DBConnection.stray_DogsEntities.Post.ToList();

            SurnameTB.TextChanged += TextBox_TextChanged;
            NameTB.TextChanged += TextBox_TextChanged;
            PatronymicTB.TextChanged += TextBox_TextChanged;
            LoginTB.TextChanged += TextBox_TextChanged;
            PasswordTB.TextChanged += TextBox_TextChanged;

            GenerateAndDisplayLogin();

            this.DataContext = this;

        }

        private void GenerateAndDisplayLogin()
        {
            for (int i = 1; i <= 100; i++)
            {
                string generatedLogin = $"д{(i).ToString("D5")}"; // Генерация логина типа a00001, a00002 и т.д.

                bool loginExists = DBConnection.stray_DogsEntities.Employee.Any(w => w.Login == generatedLogin);

                if (!loginExists)
                {
                    // Устанавливаем сгенерированный логин в текстовое поле
                    LoginTB.Text = generatedLogin;
                    break;
                }
                else if (i == 100)
                {
                    MessageBox.Show("Штаб сотрудников заполнен. Нет доступных логинов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Фамилия
            SurnameTB.Text = Regex.Replace(SurnameTB.Text, @"\s", "");
            SurnameTB.CaretIndex = SurnameTB.Text.Length;

            //Имя
            NameTB.Text = Regex.Replace(NameTB.Text, @"\s", "");
            NameTB.CaretIndex = NameTB.Text.Length;

            //Отчество
            PatronymicTB.Text = Regex.Replace(PatronymicTB.Text, @"\s", "");
            PatronymicTB.CaretIndex = PatronymicTB.Text.Length;

            //Логин
            LoginTB.Text = Regex.Replace(LoginTB.Text, @"\s", "");
            LoginTB.CaretIndex = LoginTB.Text.Length;

            //Пароль
            PasswordTB.Text = Regex.Replace(PasswordTB.Text, @"\s", "");
            PasswordTB.CaretIndex = PasswordTB.Text.Length;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^[\p{IsCyrillic}]+$"))
            {
                e.Handled = true;
            }

            TextBox textBox = (TextBox)sender;
            string currentText = textBox.Text;

            if (currentText.Length >= 50 && !string.IsNullOrEmpty(e.Text))
            {
                e.Handled = true;
                return;
            }
        }

        private void AddWorkerBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SurnameTB.Text) || string.IsNullOrWhiteSpace(NameTB.Text) || string.IsNullOrWhiteSpace(PatronymicTB.Text) ||
                        DateDP.SelectedDate == null || string.IsNullOrWhiteSpace(LoginTB.Text) || string.IsNullOrWhiteSpace(PasswordTB.Text))
                {
                    MessageBox.Show("Заполните все поля.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    employee.Surname = SurnameTB.Text.Trim();
                    employee.Name = NameTB.Text.Trim();
                    employee.Patronymic = PatronymicTB.Text.Trim();
                    employee.DateOfBirth = DateDP.SelectedDate;
                    employee.IdPost = 1;

                    string login = LoginTB.Text;
                    bool loginExists = DBConnection.stray_DogsEntities.Employee.Any(w => w.Login == login);

                    if (loginExists)
                    {
                        MessageBox.Show("Этот логин уже занят. Выберите другой.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        if (LoginTB.Text.Length > 10)
                        {
                            MessageBox.Show("Слишком длинный логин.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        else if (LoginTB.Text.Length < 6)
                        {
                            MessageBox.Show("Слишком короткий логин.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        else
                        {
                            employee.Login = LoginTB.Text.Trim();
                        }
                    }

                    if (PasswordTB.Text.Length > 10)
                    {
                        MessageBox.Show("Слишком длинный пароль.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else if (PasswordTB.Text.Length < 6)
                    {
                        MessageBox.Show("Слишком короткий пароль.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        employee.Password = PasswordTB.Text.Trim();
                    }

                    if (employee.Photo == null)
                    {
                        MessageBox.Show("Добавьте фото.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    DBConnection.stray_DogsEntities.Employee.Add(employee);
                    DBConnection.stray_DogsEntities.SaveChanges();

                    MessageBoxResult result = MessageBox.Show($"Сотрудник добавлен.", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (result == MessageBoxResult.OK)
                    {
                        NavigationService.Navigate(new MainAdminPage());
                    }
                    else { NavigationService.Navigate(new MainAdminPage()); }
                }
            }
            catch
            {
                MessageBox.Show("Непредвиденная ошибка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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
                employee.Photo = File.ReadAllBytes(openFileDialog.FileName);
                PhotoWorker.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
            TextTbBTN.Text = "Изменить фото";
        }

        private void DeletePhotoBTN_Click(object sender, RoutedEventArgs e)
        {
            PhotoWorker.Source = new BitmapImage(new Uri("/Image/person.png", UriKind.Relative));
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            DateTime? selectedDate = datePicker.SelectedDate;

            if (selectedDate.HasValue)
            {
                DateTime today = DateTime.Today;
                int age = today.Year - selectedDate.Value.Year;

                // Учитываем, прошел ли день рождения в этом году
                if (selectedDate.Value.Date > today.AddYears(-age)) age--;

                if (age < 18)
                {
                    MessageBox.Show("Сотрудник должен быть старше 18 лет.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                    datePicker.SelectedDate = null; // Сбрасываем выбранную дату
                }
            }
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите отменить все изменения?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new MainAdminPage());
            }
        }
    }
}