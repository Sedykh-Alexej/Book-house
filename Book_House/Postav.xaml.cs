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
            DGridPost.ItemsSource = Book_houseEntities.GetContext().Поставки.ToList();
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
    }
}
