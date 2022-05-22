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
    /// Логика взаимодействия для BookEdit.xaml
    /// </summary>
    public partial class BookEdit : Page
    {
        public Книги _currentКниги = new Книги();
        public BookEdit(Книги selectedКниги)
        {
            InitializeComponent();
            if (selectedКниги != null)
                _currentКниги = selectedКниги;

            DataContext = _currentКниги;
            Жанр.ItemsSource = Book_houseEntities.GetContext().Жанры.ToList();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentКниги.Название))
                errors.AppendLine("Укажите название");
            if (string.IsNullOrWhiteSpace(Цена.Text))
                errors.AppendLine("Цена не заполнена");
            if (string.IsNullOrWhiteSpace(Количество.Text))
                errors.AppendLine("Количество не заполнено");
            if (_currentКниги.Цена <= 0)
                errors.AppendLine("Укажите корректную цену");
            if (_currentКниги.Количество <= 0)
                errors.AppendLine("Количество не может быть меньше или равно 0");
            if (string.IsNullOrWhiteSpace(Жанр.Text))
                errors.AppendLine("Укажите жанр");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }


            if (_currentКниги.id == 0)
            {
                Book_houseEntities.GetContext().Книги.Add(_currentКниги);
            }

            try
            {
                Book_houseEntities.GetContext().SaveChanges();
                Manager.Forma.Navigate(new Books1());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
