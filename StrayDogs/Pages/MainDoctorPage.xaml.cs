using ClosedXML.Excel;
using LiveCharts.Wpf;
using LiveCharts;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using StrayDogs.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для MainDoctorPage.xaml
    /// </summary>
    public partial class MainDoctorPage : Page
    {
        public static List<Employee> employees { get; set; }
        public static List<Appointments> appointments { get; set; }
        public static List<Dog> dogs { get; set; }

        Employee loggedEmployee;
        public List<string> Days { get; set; }
        public SeriesCollection SeriesCollection { get; set; }

        public Func<double, string> LabelFormatter { get; set; }
        public List<string> DaysWithAppointments { get; set; } = new List<string>();
        public MainDoctorPage()
        {
            InitializeComponent();

            loggedEmployee = DBConnection.logginedEmployee;

            FioTB.Text = loggedEmployee.Surname + " " + loggedEmployee.Name + " " + loggedEmployee.Patronymic;
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

            employees = new List<Employee>(DBConnection.stray_DogsEntities.Employee.ToList());
            appointments = new List<Appointments>(DBConnection.stray_DogsEntities.Appointments.Where(x => x.IdDoctor == loggedEmployee.Id).ToList());
            dogs = new List<Dog>(DBConnection.stray_DogsEntities.Dog.ToList());

            PriemsLv.ItemsSource = appointments;

            LoadAppointmentsData();

            this.DataContext = this;
        }
        private void StajersTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        private int selectedMonth = DateTime.Now.Month;
        private int selectedYear = DateTime.Now.Year;

        private double columnWidth = 0;  // Сохраняем фиксированную ширину столбцов

        private void LoadAppointmentsData()
        {
            if (AppointmentsChart == null || MonthLabel == null)
                return;

            var context = DBConnection.stray_DogsEntities;

            var appointments = context.Appointments
                .Where(a => a.IdDoctor == loggedEmployee.Id
                            && a.Date.HasValue
                            && a.Date.Value.Month == selectedMonth
                            && a.Date.Value.Year == selectedYear)
                .ToList();

            var statuses = context.StatusAppointment
                .Select(s => s.Name)
                .ToList();

            // Получаем список дней, когда были приемы
            var daysWithAppointments = appointments
                .Select(a => a.Date.Value.Day)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            // Формируем подписи оси X в формате дд.ММ
            Days = daysWithAppointments.Select(d => $"{d:D2}.{selectedMonth:D2}").ToList();

            if (Days.Count == 0)
            {
                Days = new List<string> { "Нет данных" };
            }

            // Цвета для колонок
            var colors = new List<SolidColorBrush>
{
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#16343C")),
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8BA598")),
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9916343C"))
};

            // Устанавливаем фиксированную ширину для столбцов только один раз
            if (columnWidth == 0)
            {
                columnWidth = AppointmentsChart.ActualWidth * 0.8 / Days.Count;  // Вычисляем ширину столбца для всех дней
                columnWidth = Math.Min(Math.Max(columnWidth, 10), 50); // Ограничиваем ширину столбцов
            }

            // Очищаем график перед добавлением новых данных
            SeriesCollection = new SeriesCollection();

            // Создаем данные для столбцов
            for (int i = 0; i < statuses.Count; i++)
            {
                var status = statuses[i];

                var appointmentsByDay = appointments
                    .Where(a => a.StatusAppointment.Name == status)
                    .GroupBy(a => a.Date.Value.Day)
                    .Select(g => new { Day = g.Key, Count = g.Count() })
                    .ToList();

                var appointmentsCounts = new int[Days.Count]; // Массив для подсчета количества приемов по дням

                foreach (var day in appointmentsByDay)
                {
                    int index = Days.IndexOf($"{day.Day:D2}.{selectedMonth:D2}");
                    if (index >= 0)
                        appointmentsCounts[index] = day.Count;
                }

                SeriesCollection.Add(new ColumnSeries
                {
                    Title = status,
                    Values = new ChartValues<int>(appointmentsCounts),
                    Fill = colors[i % colors.Count],
                    MaxColumnWidth = columnWidth,  // Устанавливаем фиксированную ширину для столбцов
                    ColumnPadding = 2
                });
            }

            // Обновляем график
            AppointmentsChart.Series = SeriesCollection;

            // Обновляем заголовок месяца
            MonthLabel.Text = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(selectedMonth)} {selectedYear}";

            // Устанавливаем метки оси X (даты) на фиксированное положение
            AppointmentsChart.AxisX[0].Labels = Days;

            // Обновляем график
            AppointmentsChart.Update(true, true);
        }

        // Кнопки для переключения месяцев
        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            selectedMonth--;
            if (selectedMonth < 1)
            {
                selectedMonth = 12;
                selectedYear--;
            }
            LoadAppointmentsData();
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            selectedMonth++;
            if (selectedMonth > 12)
            {
                selectedMonth = 1;
                selectedYear++;
            }
            LoadAppointmentsData();
        }




        private void SaveExcelBT_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Сохранить график в Excel",
                FileName = "График_Приемов.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Отчет по приемам");

                var selectedMonth = DateTime.Now.Month;
                var selectedYear = DateTime.Now.Year;
                var monthName = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetMonthName(selectedMonth);

                string reportTitle = $"Отчет по приемам за {monthName} месяц {selectedYear} год";
                worksheet.Cell(1, 1).Value = reportTitle;
                worksheet.Range(1, 1, 1, SeriesCollection.Count + 1).Merge();
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(2, 2).Value = "Статусы приема";
                worksheet.Range(2, 2, 2, 4).Merge();
                worksheet.Cell(2, 2).Style.Font.Bold = true;
                worksheet.Cell(2, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(3, 1).Value = "День месяца";
                worksheet.Cell(3, 2).Value = "Первичный";
                worksheet.Cell(3, 3).Value = "Повторный";
                worksheet.Cell(3, 4).Value = "Завершающий";

                worksheet.Row(3).Style.Font.Bold = true;

                for (int i = 0; i < Days.Count; i++)
                {
                    worksheet.Cell(i + 4, 1).Value = Days[i];

                    for (int j = 0; j < SeriesCollection.Count; j++)
                    {
                        var series = SeriesCollection[j];
                        worksheet.Cell(i + 4, j + 2).Value = Convert.ToInt32(series.Values[i]);
                    }
                }

                int totalRow = Days.Count + 5;
                worksheet.Cell(totalRow, 1).Value = "Общее количество приемов";
                worksheet.Range(totalRow, 1, totalRow, 1).Style.Font.Bold = true;

                for (int j = 0; j < SeriesCollection.Count; j++)
                {
                    var total = SeriesCollection[j].Values.Cast<int>().Sum();
                    worksheet.Cell(totalRow, j + 2).Value = total;
                    worksheet.Cell(totalRow, j + 2).Style.Font.Bold = true;
                }

                int grandTotal = 0;

                for (int i = 0; i < SeriesCollection.Count; i++)
                {
                    var series = SeriesCollection[i];
                    var total = series.Values.Cast<int>().Sum();
                    grandTotal += total;

                    worksheet.Cell(totalRow, i + 2).Value = total;
                    worksheet.Cell(totalRow, i + 2).Style.Font.Bold = true;
                }

                int grandTotalRow = totalRow + 1;
                worksheet.Cell(grandTotalRow, 1).Value = "Общая сумма приемов";
                worksheet.Range(grandTotalRow, 1, grandTotalRow, SeriesCollection.Count).Merge();
                worksheet.Cell(grandTotalRow, 1).Style.Font.Bold = true;
                worksheet.Cell(grandTotalRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(grandTotalRow, SeriesCollection.Count + 1).Value = grandTotal;
                worksheet.Cell(grandTotalRow, SeriesCollection.Count + 1).Style.Font.Bold = true;

                worksheet.Columns().AdjustToContents();

                MessageBox.Show("График успешно сохранен в Excel!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                workbook.SaveAs(saveFileDialog.FileName);
            }
        }

        private void SavePDFBT_Click(object sender, RoutedEventArgs e)
        {
            var renderBitmap = new RenderTargetBitmap(
                (int)AppointmentsChart.ActualWidth * 3,
                (int)AppointmentsChart.ActualHeight * 3,
                300d, 300d,
                PixelFormats.Pbgra32);

            renderBitmap.Render(AppointmentsChart);

            using (var pngStream = new MemoryStream())
            {
                var pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                pngEncoder.Save(pngStream);
                pngStream.Position = 0;

                var selectedMonth = DateTime.Now.ToString("MMMM");
                var selectedYear = DateTime.Now.Year;

                var pdfDocument = new PdfDocument();
                var title = $"Отчет по приемам за {selectedMonth} месяц {selectedYear} года";
                pdfDocument.Info.Title = title;

                var pdfPage = pdfDocument.AddPage();
                pdfPage.Width = XUnit.FromPoint(renderBitmap.PixelWidth / 3);
                pdfPage.Height = XUnit.FromPoint(renderBitmap.PixelHeight / 3);

                using (var gfx = XGraphics.FromPdfPage(pdfPage))
                {
                    var xImage = XImage.FromStream(pngStream);
                    gfx.DrawImage(xImage, 0, 0, pdfPage.Width, pdfPage.Height);
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    Title = "Сохранить график в PDF",
                    FileName = "График_Приемов.pdf"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    pdfDocument.Save(saveFileDialog.FileName);
                    MessageBox.Show("График успешно сохранен в PDF!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}