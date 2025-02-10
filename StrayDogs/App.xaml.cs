using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using System.Windows.Media;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace StrayDogs
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Настройка основной темы
            var paletteHelper = new PaletteHelper();
            var baseTheme = BaseTheme.Light;
            var primaryColor = (Color)FindResource("PrimaryHueMid");
            var secondaryColor = (Color)FindResource("SecondaryAccentMid");

            // Создание и применение темы
            var theme = Theme.Create(baseTheme, primaryColor, secondaryColor);
            paletteHelper.SetTheme(theme);

            // Установка других кистей (например, фона)
            Application.Current.Resources["MaterialDesignPaper"] = FindResource("BackgroundPrimaryBrush");
            Application.Current.Resources["MaterialDesignDivider"] = FindResource("BorderColorBrush");
        }
    }
}
