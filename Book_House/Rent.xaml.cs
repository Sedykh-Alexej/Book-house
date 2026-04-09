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
            DGridRent.ItemsSource = Book_houseEntities.GetContext().Продажа_книг.ToList();
        }

        private void BtnEdit_click(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new RentEdit((sender as Button).DataContext as Продажа_книг));
        }

        private void BtnAdd_click(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new RentEdit(null));
        }

        private void BtnDelete_click(object sender, RoutedEventArgs e)
        {
            var PostavForRemoving = DGridRent.SelectedItems.Cast<Продажа_книг>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующее {PostavForRemoving.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Book_houseEntities.GetContext().Продажа_книг.RemoveRange(PostavForRemoving);
                    Book_houseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    DGridRent.ItemsSource = Book_houseEntities.GetContext().Продажа_книг.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Сashier());
        }
    }
}
