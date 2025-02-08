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
                        aviary.Number = int.Parse(NumberTB.Text.Trim());
                    }
                    aviary.Square = int.Parse(SquareTB.Text.Trim());

                    var a = TypeCB.SelectedItem as TypeAviary;
                    aviary.IdType = aviary.Id;

                    DBConnection.stray_DogsEntities.Aviary.Add(aviary);
                    DBConnection.stray_DogsEntities.SaveChanges();
                    MessageBox.Show("Вы добавили новый вольер. Вернуться на главную?", "Успех!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    MessageBoxResult messageBoxResult = new MessageBoxResult();
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        NavigationService.Navigate(new MainAdminPage());
                    }
                    else { }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка. Повторите снова.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainAdminPage());
        }
    }
}
