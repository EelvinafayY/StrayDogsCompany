using DocumentFormat.OpenXml.InkML;
using Microsoft.Win32;
using StrayDogs.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.RightsManagement;
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
    /// Логика взаимодействия для EditEmployeePage.xaml
    /// </summary>
    public partial class EditEmployeePage : Page
    {
        public static List<Employee> workers { get; set; }
        public static List<Post> posts { get; set; }

        Employee contextWorker;

        public EditEmployeePage(Employee worker)
        {
            InitializeComponent();
            contextWorker = worker;
            InitializeDataInPage();
            this.DataContext = contextWorker;

            SurnameTB.TextChanged += TextBox_TextChanged;
            NameTB.TextChanged += TextBox_TextChanged;
            PatronymicTB.TextChanged += TextBox_TextChanged;
            PasswordTB.TextChanged += TextBox_TextChanged;
        }

        private void InitializeDataInPage()
        {
            workers = DBConnection.stray_DogsEntities.Employee.ToList();
            posts = DBConnection.stray_DogsEntities.Post.ToList();
            PostCB.ItemsSource = posts;
            SurnameTB.Text = contextWorker.Surname;
            NameTB.Text = contextWorker.Name;
            PatronymicTB.Text = contextWorker.Patronymic;
            DateDP.SelectedDate = contextWorker.DateOfBirth;
            PostCB.SelectedIndex = (int)contextWorker.IdPost - 1;
            LoginTB.Text = contextWorker.Login;
            PasswordTB.Text = contextWorker.Password;

            if (contextWorker.Photo != null)
            {
                using (var stream = new MemoryStream(contextWorker.Photo))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    PhotoWorker.Source = bitmap;
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

        private void SaveBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SurnameTB.Text) || string.IsNullOrWhiteSpace(NameTB.Text) || string.IsNullOrWhiteSpace(PatronymicTB.Text) ||
                        DateDP.SelectedDate == null || string.IsNullOrWhiteSpace(LoginTB.Text) || string.IsNullOrWhiteSpace(PasswordTB.Text) || PostCB.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все поля.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    contextWorker.Surname = SurnameTB.Text.Trim();
                    contextWorker.Name = NameTB.Text.Trim();
                    contextWorker.Patronymic = PatronymicTB.Text.Trim();
                    contextWorker.DateOfBirth = DateDP.SelectedDate;

                    var p = PostCB.SelectedItem as Post;
                    contextWorker.IdPost = p.Id;

                    string login = LoginTB.Text;
                    bool loginExists = DBConnection.stray_DogsEntities.Employee.Any(w => w.Login == login && w.Id != contextWorker.Id);

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
                            contextWorker.Login = LoginTB.Text.Trim();
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
                        contextWorker.Password = PasswordTB.Text.Trim();
                    }

                    if (contextWorker.Photo == null)
                    {
                        MessageBox.Show("Добавьте фото.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    DBConnection.stray_DogsEntities.SaveChanges();
                    MessageBox.Show("Данные сохранены.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new MainAdminPage());
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
                contextWorker.Photo = File.ReadAllBytes(openFileDialog.FileName);
                PhotoWorker.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void DeletePhotoBTN_Click(object sender, RoutedEventArgs e)
        {
            PhotoWorker.Source = new BitmapImage(new Uri("/Image/person.png", UriKind.Relative));
            contextWorker.Photo = null;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            DateTime? selectedDate = datePicker.SelectedDate;

            if (selectedDate.HasValue)
            {
                DateTime today = DateTime.Today;
                int age = today.Year - selectedDate.Value.Year;

                if (selectedDate.Value.Date > today.AddYears(-age)) age--;

                if (age < 18)
                {
                    MessageBox.Show("Сотрудник должен быть старше 18 лет.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                    datePicker.SelectedDate = null;
                }
            }
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите отменить все изменения?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                DBConnection.stray_DogsEntities.Entry(contextWorker).Reload();
                NavigationService.Navigate(new MainAdminPage());
            }
        }
    }
}
