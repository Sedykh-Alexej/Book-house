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
    /// Логика взаимодействия для Rent.xaml
    /// </summary>
    public partial class Rent : Page
    {
        public Rent()
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

        private void BtnEdit_click(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new RentEdit((sender as Button).DataContext as Книги_в_аренде));
        }

        private void BtnAdd_click(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new RentEdit(null));
        }

        private void BtnDelete_click(object sender, RoutedEventArgs e)
        {
            var PostavForRemoving = DGridRent.SelectedItems.Cast<Книги_в_аренде>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующее {PostavForRemoving.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Book_houseEntities.GetContext().Книги_в_аренде.RemoveRange(PostavForRemoving);
                    Book_houseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    DGridRent.ItemsSource = Book_houseEntities.GetContext().Книги_в_аренде.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Cashier());
        }
    }
}
