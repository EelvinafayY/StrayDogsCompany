using System;
using System.Collections.Generic;
using System.IO;
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
using StrayDogs.DB;

namespace StrayDogs.Pages
{
    /// <summary>
    /// Логика взаимодействия для AllDogsPage.xaml
    /// </summary>
    public partial class AllDogsPage : Page
    {
        public static List<Dog> dogs {  get; set; }
        public static List<Employee> employees { get; set; }
        public AllDogsPage()
        {
            InitializeComponent();
            dogs = new List<Dog>(DBConnection.stray_DogsEntities.Dog.ToList());
            FIOTB.Text = DBConnection.logginedEmployee.Surname + " " + DBConnection.logginedEmployee.Name + " " + DBConnection.logginedEmployee.Patronymic;
            if (DBConnection.logginedEmployee.Photo != null)
            {
                using (var stream = new MemoryStream(DBConnection.logginedEmployee.Photo))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    PhotoEmpl.Source = bitmap;
                }
            }
            DogsLv.ItemsSource = dogs;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
