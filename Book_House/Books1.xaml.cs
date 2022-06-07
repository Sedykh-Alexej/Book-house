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
    /// Логика взаимодействия для Books1.xaml
    /// </summary>
    public partial class Books1 : Page
    {
        public Books1()
        {
            InitializeComponent();
            Жанр.ItemsSource = Book_houseEntities.GetContext().Жанры.ToList();
            Автор.ItemsSource = Book_houseEntities.GetContext().Авторы.ToList();

        }
        private void BtnEdit_click(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new BookEdit((sender as Button).DataContext as Книги));
        }

        private void BtnAdd_click(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new BookEdit(null));
        }

        private void BtnDelete_click(object sender, RoutedEventArgs e)
        {
            var PostavForRemoving = DGridBook.SelectedItems.Cast<Книги>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующее {PostavForRemoving.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Book_houseEntities.GetContext().Книги.RemoveRange(PostavForRemoving);
                    Book_houseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    DGridBook.ItemsSource = Book_houseEntities.GetContext().Книги.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Accountant());
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

        private void Обновить2(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Автор.Text))
                errors.AppendLine("Укажите автора");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                var Avtor = Book_houseEntities.GetContext().Авторы.Where(d => d.Автор1 == Автор.Text).FirstOrDefault();
                DGridBook.ItemsSource = Book_houseEntities.GetContext().Книги.Where(d => d.Автор == Avtor.id_Автора).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Все(object sender, RoutedEventArgs e)
        {
            DGridBook.ItemsSource = Book_houseEntities.GetContext().Книги.ToList();
        }
    }
}
