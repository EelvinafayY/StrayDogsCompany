using MaterialDesignThemes.Wpf;
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
using System.Windows.Shapes;

namespace StrayDogs.Windows
{
    /// <summary>
    /// Логика взаимодействия для GuestApplicationWindow.xaml
    /// </summary>
    public partial class GuestApplicationWindow : Window
    {
        public static List<Aplication> aplications { get; set; }

        Aplication aplication = new Aplication();

        Dog contextDog;
        public GuestApplicationWindow(Dog dog)
        {
            InitializeComponent();

            aplications = new List<Aplication>(DBConnection.stray_DogsEntities.Aplication.ToList());

            DataContext = contextDog = dog;
        }

        private void AutoSuggestBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"\d");
        }

        private void AutoSuggestBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void AutoSuggestBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is AutoSuggestBox autoSuggestBox)
            {
                string phoneNumber = new string(autoSuggestBox.Text.Where(char.IsDigit).ToArray());

                if (phoneNumber.Length > 11)
                    phoneNumber = phoneNumber.Substring(0, 11);

                autoSuggestBox.Text = FormatPhoneNumber(phoneNumber);
                autoSuggestBox.CaretIndex = autoSuggestBox.Text.Length;
            }
        }

        private string FormatPhoneNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            if (input.Length >= 1)
                input = "+7 (" + input.Substring(1);

            if (input.Length >= 7)
                input = input.Insert(7, ") ");

            if (input.Length >= 12)
                input = input.Insert(12, "-");

            if (input.Length >= 15)
                input = input.Insert(15, "-");

            return input;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var applicationDog = DBConnection.stray_DogsEntities.Aplication.FirstOrDefault(x => x.IDDog == contextDog.Id
                && x.Phone == PhoneTbx.Text.Trim());
                StringBuilder error = new StringBuilder();

                if (string.IsNullOrWhiteSpace(PhoneTbx.Text))
                {
                    error.AppendLine("Заполните поле!");
                }
                if (applicationDog != null)
                {
                    MessageBox.Show("Вы уже отправили заявку на данную собаку!");
                    Close();
                }
                else
                {
                    aplication.Phone = PhoneTbx.Text;
                    aplication.IDDog = contextDog.Id;
                    aplication.IDStatusAplication = 3;


                    DBConnection.stray_DogsEntities.Aplication.Add(aplication);
                    DBConnection.stray_DogsEntities.SaveChanges();

                    Close();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка.");
            }
        }
    }
}
