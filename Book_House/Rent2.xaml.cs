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
                DateTime Date1 = Convert.ToDateTime(Дата1.Text);
                DateTime Date2 = Convert.ToDateTime(Дата2.Text);
                DGridRent.ItemsSource = Book_houseEntities.GetContext().Книги_в_аренде.Where(d => d.Дата_получения > Date1 && d.Дата_получения < Date2).ToList();
                if (DGridRent != null)
                {
                    int Sum;
                    Sum = (int)Book_houseEntities.GetContext().Книги_в_аренде.Where(d => d.Дата_получения > Date1 && d.Дата_получения < Date2).Sum(c => c.К_оплате);
                    Itog.Text = "Прибыль составляет: " + Sum + " рублей";
                }
                else
            {
                    Itog.Text = "Прибыль составляет: " + "0 рублей";
                }

        }
            catch (Exception)
            {
                MessageBox.Show("Нет записей за текущий период. Пожалуйста, проверьте формат даты: число.месяц.год");
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Chief());
        }
    }
}
