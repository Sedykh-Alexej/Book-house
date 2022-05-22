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
    /// Логика взаимодействия для Клиенты1.xaml
    /// </summary>
    public partial class Клиенты1 : Page
    {
        public Клиенты1()
        {
            InitializeComponent();
            DGridClient.ItemsSource = Book_houseEntities.GetContext().Клиенты.ToList();
        }

        private void BtnEdit_click(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new EditingClients((sender as Button).DataContext as Клиенты));
        }

        private void BtnAdd_click(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new EditingClients(null));
        }

        private void BtnDelete_click(object sender, RoutedEventArgs e)
        {
            var PostavForRemoving = DGridClient.SelectedItems.Cast<Клиенты>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующее {PostavForRemoving.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Book_houseEntities.GetContext().Клиенты.RemoveRange(PostavForRemoving);
                    Book_houseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    DGridClient.ItemsSource = Book_houseEntities.GetContext().Клиенты.ToList();
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
