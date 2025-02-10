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
    /// Логика взаимодействия для MainDoctorPage.xaml
    /// </summary>
    public partial class MainDoctorPage : Page
    {
        public MainDoctorPage()
        {
            InitializeComponent();
            List<string> list = new List<string>() { "Первый", "Second", "Third", "Fourht", "Five" };
            PriemsLv.ItemsSource = list;
        }
        private void StajersTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

    
    }
}
