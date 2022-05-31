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
using Excel = Microsoft.Office.Interop.Excel;


namespace Book_House
{
    /// <summary>
    /// Логика взаимодействия для Rent2.xaml
    /// </summary>
    public partial class Rent2 : Page
    {
        public Rent2()
        {
            InitializeComponent();
        }

        private void Status(object sender, RoutedEventArgs e)
        {
            try
            {
                DGridRent.ItemsSource = Book_houseEntities.GetContext().Книги_в_аренде.Where(d => d.Статус == 1).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Просроченные_книги(object sender, RoutedEventArgs e)
        {
            try
            {
                DGridRent.ItemsSource = Book_houseEntities.GetContext().Книги_в_аренде.Where(d => (d.Статус == 1) && d.Дата_возврата < DateTime.Today).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Все(object sender, RoutedEventArgs e)
        {
            DGridRent.ItemsSource = Book_houseEntities.GetContext().Книги_в_аренде.ToList();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Chief());
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            var Member = Book_houseEntities.GetContext().Книги_в_аренде.ToList().OrderBy(p => p.id_Сотрудника).ToList();
            var application = new Excel.Application();
            application.SheetsInNewWorkbook = Member.Count();
            Excel.Workbook workbook = application.Workbooks.Add(Type.Missing);

            int startRowIndex = 1;
            for (int i = 0; i < Member.Count(); i++)
            {
                Excel.Worksheet worksheet = application.Worksheets.Item[i + 1];
                worksheet.Name = Member[i].Сотрудники.Фамилия + " " + Member[i].Сотрудники.Имя + Member[i].Сотрудники.Отчество;

                worksheet.Cells[1][startRowIndex] = "Дата получения";
                worksheet.Cells[1][startRowIndex] = "Дата возврата";
                worksheet.Cells[1][startRowIndex] = "Фактическая дата возврата";
                worksheet.Cells[1][startRowIndex] = "Фамилия клиента";
                worksheet.Cells[1][startRowIndex] = "Имя клиента";
                worksheet.Cells[1][startRowIndex] = "Отчество клиента";
                worksheet.Cells[1][startRowIndex] = "Книга";
                worksheet.Cells[1][startRowIndex] = "Количество";
                startRowIndex++;

                var Categories = Book_houseEntities.GetContext().Книги_в_аренде.ToList();
                foreach (var groupCategory in Categories)
                {
                    Excel.Range headerRange = worksheet.Range[worksheet.Cells[1][startRowIndex], worksheet.Cells[8][startRowIndex]];
                    headerRange.Merge();
                    headerRange.Value = groupCategory.id;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Italic = true;

                    startRowIndex++;

                    foreach (var payment in Categories)
                    {
                        worksheet.Cells[1][startRowIndex] = payment.Дата_получения.ToString("dd.MM.yyyy");
                        worksheet.Cells[2][startRowIndex] = payment.Дата_возврата.ToString("dd.MM.yyyy");
                        worksheet.Cells[3][startRowIndex] = payment.Фактическая_дата_возврата;
                        worksheet.Cells[4][startRowIndex] = payment.Клиенты.Фамилия;
                        worksheet.Cells[5][startRowIndex] = payment.Клиенты.Имя;
                        worksheet.Cells[6][startRowIndex] = payment.Клиенты.Отчество;
                        worksheet.Cells[7][startRowIndex] = payment.Книги.Название;
                        worksheet.Cells[8][startRowIndex] = payment.Количество;
                    }
                }




            }
        }
    }
}
