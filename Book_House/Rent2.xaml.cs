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

        private void Все(object sender, RoutedEventArgs e)
        {
            try
            {
                DGridRent.ItemsSource = Book_houseEntities.GetContext().Книги_в_аренде.Where(d => d.Статус == 1).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            int a;
            a = (int)Book_houseEntities.GetContext().Книги_в_аренде.Sum(c => c.Количество);
        }
    }
}
