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
using Microsoft.Office.Interop.Excel;


namespace Book_House
{
    /// <summary>
    /// Логика взаимодействия для Rent2.xaml
    /// </summary>
    public partial class Rent2 : System.Windows.Controls.Page
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
            Excel.Application excel = new Excel.Application();
            excel.Visible = true; 
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < DGridRent.Columns.Count; j++) 
            {
                Range myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true; 
                sheet1.Columns[j + 1].ColumnWidth = 15; 
                myRange.Value2 = DGridRent.Columns[j].Header;
            }
            for (int i = 0; i < DGridRent.Columns.Count; i++)
            { 
                for (int j = 0; j < DGridRent.Items.Count; j++)
                {
                    TextBlock b = DGridRent.Columns[i].GetCellContent(DGridRent.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
        }
    }
}
