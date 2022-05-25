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
            Фамилия.ItemsSource = Book_houseEntities.GetContext().Клиенты.ToList();
            Имя.ItemsSource = Book_houseEntities.GetContext().Клиенты.ToList();
            Отчество.ItemsSource = Book_houseEntities.GetContext().Клиенты.ToList();
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

        private void Все(object sender, RoutedEventArgs e)
        {
            DGridClient.ItemsSource = Book_houseEntities.GetContext().Клиенты.ToList();
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Фамилия.Text))
                errors.AppendLine("Укажите Фамилию");
            if (string.IsNullOrWhiteSpace(Имя.Text))
                errors.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(Отчество.Text))
                errors.AppendLine("Укажите отчество");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                var Клиент = Book_houseEntities.GetContext().Клиенты.Where(d => d.Фамилия == Фамилия.Text && d.Имя == Имя.Text && d.Отчество == Отчество.Text).FirstOrDefault();
                if (Клиент != null)
                {
                    DGridClient.ItemsSource = Book_houseEntities.GetContext().Клиенты.Where(d => d.id == Клиент.id).ToList();
                }
                else {
                    MessageBox.Show("Не существует клиента с данным именем, фамилией и отчеством");
                }
            }

                
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
