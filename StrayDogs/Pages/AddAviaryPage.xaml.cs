using DocumentFormat.OpenXml.Drawing.Diagrams;
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
    /// Логика взаимодействия для AddAviaryPage.xaml
    /// </summary>
    public partial class AddAviaryPage : Page
    {
        public static Aviary aviary = new Aviary();
        public static List<TypeAviary> typeAviaries { get; set; }

        public AddAviaryPage()
        {
            InitializeComponent();
            typeAviaries = DBConnection.stray_DogsEntities.TypeAviary.ToList();
            this.DataContext = this;
        }

        private void SaveBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NumberTB.Text) || string.IsNullOrWhiteSpace(SquareTB.Text) || TypeCB.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все поля.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    string number = NumberTB.Text;
                    bool numberExists = DBConnection.stray_DogsEntities.Aviary.Any(i => i.Number.ToString() == number);

                    if (numberExists)
                    {
                        MessageBox.Show("Вольер с таким номером уже существует.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (long.TryParse(NumberTB.Text.Trim(), out long numberVolier) && numberVolier > Int32.MaxValue)
                        {
                            MessageBox.Show("Вы превысили значение номера вольера.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        else
                            aviary.Number = int.Parse(NumberTB.Text.Trim());

                        if (int.Parse(SquareTB.Text.Trim()) > 100)
                        {
                            MessageBox.Show("Вы превысили значение площади вольера. Максимальное значение вольера равно 100", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        else
                            aviary.Square = int.Parse(SquareTB.Text.Trim());

                        var a = TypeCB.SelectedItem as TypeAviary;
                        aviary.IdType = a.Id;

                        DBConnection.stray_DogsEntities.Aviary.Add(aviary);
                        DBConnection.stray_DogsEntities.SaveChanges();
                        MessageBoxResult result = MessageBox.Show("Вы добавили новый вольер. Вернуться на главную?", "Успех!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                            NavigationService.Navigate(new MainAdminPage());

                        if (result == MessageBoxResult.No)
                        {
                            NumberTB.Text = "";
                            SquareTB.Text = "";
                            TypeCB.SelectedItem = null;
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка. Повторите снова.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void RegexValidation(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);

            if (textBox.Text.Length == 0 && e.Text == "0")
            {
                e.Handled = true;
            }
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите выйти?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new MainAdminPage());
            }
        }
    }
}
