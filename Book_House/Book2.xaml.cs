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
    /// Логика взаимодействия для Book2.xaml
    /// </summary>
    public partial class Book2 : Page
    {
        public Book2()
        {
            InitializeComponent();
            Жанр.ItemsSource = Book_houseEntities.GetContext().Жанры.ToList();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Cashier());
        }

        private void Обновить(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Жанр.Text))
                errors.AppendLine("Укажите жанр");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                var Жанрр = Book_houseEntities.GetContext().Жанры.Where(d => d.Наименование == Жанр.Text).FirstOrDefault();
                DGridBook.ItemsSource = Book_houseEntities.GetContext().Книги.Where(d => d.Жанр == Жанрр.id).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
