﻿using StrayDogs.DB;
using StrayDogs.Windows;
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
    /// Логика взаимодействия для GuestDogPage.xaml
    /// </summary>
    public partial class GuestDogPage : Page
    {
        Dog contextDog;
        public GuestDogPage(Dog dog)
        {
            InitializeComponent();
            var dogs = DB.DBConnection.stray_DogsEntities.Dog.ToList();
            DataContext = contextDog = dog;
        }

        private void GuestApplicationBTN_Click(object sender, RoutedEventArgs e)
        {

            GuestApplicationWindow guestApplicationWindow = new GuestApplicationWindow(contextDog);
            guestApplicationWindow.ShowDialog();
            NavigationService.Navigate(new GuestHomePage());
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GuestHomePage());
        }
    }
}
