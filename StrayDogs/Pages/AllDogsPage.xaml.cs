using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static System.Net.Mime.MediaTypeNames;

namespace StrayDogs.Pages
{
    /// <summary>
    /// Логика взаимодействия для AllDogsPage.xaml
    /// </summary>
    public partial class AllDogsPage : Page
    {
        public static List<Dog> dogs {  get; set; }
        public static List<Employee> employees { get; set; }
        public static List<Post> posts { get; set; }

        Employee loginedEmployee;

        public AllDogsPage()
        {
            InitializeComponent();

            Refresh(0);

            dogs = DBConnection.stray_DogsEntities.Dog.ToList();
            posts = DBConnection.stray_DogsEntities.Post.ToList();
            loginedEmployee = DBConnection.logginedEmployee;
            FioTB.Text = loginedEmployee.Surname + " " + loginedEmployee.Name + " " + loginedEmployee.Patronymic;
            if (DBConnection.logginedEmployee.Photo != null)
            {
                using (var stream = new MemoryStream(DBConnection.logginedEmployee.Photo))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    WorkerPhoto.Source = bitmap;
                }
            }


            DogsLv.ItemsSource = dogs;


            this.DataContext = this;

        }

        private void InfoDogPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Refresh(int i) //Поиск по НомеруСобаки, описанию, клетке и наименованию типа клетки
        {
            var alldogs = DBConnection.stray_DogsEntities.Dog.ToList();
            var filtered = alldogs.AsQueryable();

            var searchText = SearchDogsTB.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filtered = filtered.Where(x => x.OrdinalNumber.ToLower().Contains(searchText) ||
                                               x.Gender.Name.ToLower().Contains(searchText) ||
                                               x.Description.ToLower().Contains(searchText) ||
                                               (x.Aviary != null && x.Aviary.TypeAviary != null &&
                                                x.Aviary.TypeAviary.Name.ToLower().Contains(searchText))); // добавлен поиск по типу клетки
            }

            DogsLv.ItemsSource = filtered.ToList();
        }


        private void SearchDogsTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh(0);
        }
    }
}
