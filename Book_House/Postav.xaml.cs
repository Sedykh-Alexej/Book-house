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
    /// Логика взаимодействия для Postav.xaml
    /// </summary>
    public partial class Postav : Page
    {
        public Postav()
        {
            InitializeComponent();
            Поставщики.ItemsSource = Book_houseEntities.GetContext().Поставщики.ToList();
            Книги.ItemsSource = Book_houseEntities.GetContext().Книги.ToList();
        }

        private void BtnAdd_click(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new EditPostav());
        }



        private void BtnDelete_click(object sender, RoutedEventArgs e)
        {
            var PostForRemoving = DGridPost.SelectedItems.Cast<Поставки>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующее {PostForRemoving.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Book_houseEntities.GetContext().Поставки.RemoveRange(PostForRemoving);
                    Book_houseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    DGridPost.ItemsSource = Book_houseEntities.GetContext().Поставки.ToList();
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

        private void Все(object sender, RoutedEventArgs e)
        {
            DGridPost.ItemsSource = Book_houseEntities.GetContext().Поставки.ToList();
        }

        private void Обновить(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Поставщики.Text))
                errors.AppendLine("Укажите поставщика");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                var Поставщик = Book_houseEntities.GetContext().Поставщики.Where(d => d.Наименование == Поставщики.Text).FirstOrDefault();
                DGridPost.ItemsSource = Book_houseEntities.GetContext().Поставки.Where(d => d.id_Поставщика == Поставщик.id).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Обновить2(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Книги.Text))
                errors.AppendLine("Укажите книгу");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                var Книга = Book_houseEntities.GetContext().Книги.Where(d => d.Название == Книги.Text).FirstOrDefault();
                DGridPost.ItemsSource = Book_houseEntities.GetContext().Поставки.Where(d => d.id_Книги == Книга.id).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
