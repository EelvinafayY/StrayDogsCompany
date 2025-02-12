using MaterialDesignThemes.Wpf;
using StrayDogs.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace StrayDogs.Pages
{
    /// <summary>
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {

        public string SurnameBoxText { get; set; }
        public string NameBoxText { get; set; }
        public string PatronymicBoxText { get; set; }
        public string LoginBoxText { get; set; }
        public string PasswordBoxText { get; set; }
        public ObservableCollection<SuggestionItem> SurnameBoxSuggestions { get; set; }
        public ObservableCollection<SuggestionItem> NameBoxSuggestions { get; set; }
        public ObservableCollection<SuggestionItem> PatronymicBoxSuggestions { get; set; }
        public ObservableCollection<SuggestionItem> LoginBoxSuggestions { get; set; }
        public ObservableCollection<SuggestionItem> PasswordBoxSuggestions { get; set; }

        public bool IsClearButtonVisible { get; set; }

        public static List<Employee> employees {  get; set; }
        public static List<Post> posts { get; set; }

        Employee loginedEmployee;

        public AccountPage()
        {
            InitializeComponent();

            loginedEmployee = DBConnection.logginedEmployee;
            IsClearButtonVisible = loginedEmployee.IdPost != 2;
            InfoView();


            this.DataContext = this;
        }

        public void InfoView()
        {
            if (DBConnection.logginedEmployee.IdPost == 1)
            {
                IsReadOnlyTrue();
            }

            employees = DBConnection.stray_DogsEntities.Employee.ToList();
            posts = DBConnection.stray_DogsEntities.Post.ToList();

            SurnameBoxText = loginedEmployee.Surname;
            NameBoxText = loginedEmployee.Name;
            PatronymicBoxText = loginedEmployee.Patronymic;
            LoginBoxText = loginedEmployee.Login;
            PasswordBoxText = loginedEmployee.Password;

            BHDP.Text = loginedEmployee.DateOfBirth.ToString();
            PostCB.SelectedIndex = (int)loginedEmployee.IdPost - 1;

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
        }

        public void IsReadOnlyTrue()
        {

            OKBTN.Visibility = Visibility;

            SurnameTB.IsReadOnly = false;
            NameTB.IsReadOnly = false;
            PatrNameTB.IsReadOnly = false;
            LoginTB.IsReadOnly = false;
            PasswordTB.IsReadOnly = false;

            BHDP.IsEnabled = true;

            PostCB.IsEnabled = true;
            PostCB.IsReadOnly = false;

        }

        public class SuggestionItem
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        private void OKBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loginedEmployee.Name = NameBoxText;
                DBConnection.stray_DogsEntities.SaveChanges();
            }
            catch 
            {
                MessageBox.Show("Непредвиденная ошибка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }
    }
}
