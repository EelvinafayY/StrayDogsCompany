﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для DoctorMenuPage.xaml
    /// </summary>
    public partial class DoctorMenuPage : Page
    {
        public DoctorMenuPage()
        {
            InitializeComponent();
            ContentDoctorFrame.NavigationService.Navigate(new MainDoctorPage());
        }

        private void HomeTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ContentDoctorFrame.NavigationService.Navigate(new MainDoctorPage());
        }

        private void DogTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ContentDoctorFrame.NavigationService.Navigate(new AllDogsPage());
        }

        private void AccountTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ContentDoctorFrame.NavigationService.Navigate(new AccountPage());
        }

        private void ExitTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите выйти из системы?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                //ПЕРЕЗАПУСК ПРОГРАММЫ
                string exePath = Process.GetCurrentProcess().MainModule.FileName;
                Process.Start(exePath);
                Application.Current.Shutdown();
            }
        }
    }
}
