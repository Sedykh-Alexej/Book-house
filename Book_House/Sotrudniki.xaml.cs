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
    /// Логика взаимодействия для Sotrudniki.xaml
    /// </summary>
    public partial class Sotrudniki : Page
    {
        public Sotrudniki()
        {
            InitializeComponent();
            DGridSales.ItemsSource = Book_houseEntities.GetContext().Сотрудники.ToList();
        }

        private void BtnAdd_click(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new EditSotrudniki(null));
        }

        private void BtnEdit_click(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new EditSotrudniki((sender as Button).DataContext as Сотрудники));
        }

        private void BtnDelete_click(object sender, RoutedEventArgs e)
        {
            var SalesForRemoving = DGridSales.SelectedItems.Cast<Сотрудники>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующее {SalesForRemoving.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Book_houseEntities.GetContext().Сотрудники.RemoveRange(SalesForRemoving);
                    Book_houseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    DGridSales.ItemsSource = Book_houseEntities.GetContext().Сотрудники.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Chief());
        }
    }
}
