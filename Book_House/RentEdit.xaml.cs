using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для RentEdit.xaml
    /// </summary>
    public partial class RentEdit : Page
    {
        public Продажа_книг _currentКниги_в_продаже = new Продажа_книг();
        public RentEdit(Продажа_книг selectedКниги_в_продаже)
        {
            InitializeComponent();
            if (selectedКниги_в_продаже != null)
                _currentКниги_в_продаже = selectedКниги_в_продаже;

            DataContext = _currentКниги_в_продаже;
            var context = Book_houseEntities.GetContext();
            var клиенты = context.Клиенты.ToList();
            КлиентИмя.ItemsSource = клиенты;
            КлиентФамилия.ItemsSource = клиенты;
            КлиентОтчество.ItemsSource = клиенты;
            Книга.ItemsSource = context.Книги.ToList();
            Сотрудник.Text = Manager.IFO;

            Статус.Text = _currentКниги_в_продаже.Статус == 0
                ? "В аренде"
                : _currentКниги_в_продаже.Статус1?.Название ?? string.Empty;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            var context = Book_houseEntities.GetContext();

            if (string.IsNullOrWhiteSpace(Книга.Text))
                errors.AppendLine("Укажите название книги");
            if (_currentКниги_в_продаже.id_Клиента <= 0)
                errors.AppendLine("Укажите клиента");

            if (string.IsNullOrWhiteSpace(Количество.Text))
                errors.AppendLine("Укажите количество");

            if (!long.TryParse(Количество.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out long parsedКол))
                errors.AppendLine("Количество должно быть целым числом");

            if (parsedКол <= 0)
                errors.AppendLine("Количество не может быть меньше или равно 0");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentКниги_в_продаже.Количество = (int)parsedКол;

            try
            {
                var book = context.Книги.FirstOrDefault(d => d.id == _currentКниги_в_продаже.id_Книги);
                if (book == null)
                {
                    MessageBox.Show("Выбранная книга не найдена в базе");
                    return;
                }

                if (_currentКниги_в_продаже.id == 0)
                {
                    _currentКниги_в_продаже.id_Сотрудника = Manager.IDSotr;
                    _currentКниги_в_продаже.Дата_продажи = DateTime.Today;
                    _currentКниги_в_продаже.Статус = 1;
                    context.Продажа_книг.Add(_currentКниги_в_продаже);

                    if (book.Количество < _currentКниги_в_продаже.Количество)
                    {
                        MessageBox.Show("На складе нет столько книг");
                        return;
                    }

                    book.Количество -= _currentКниги_в_продаже.Количество;
                }
                else
                {
                    if (book.Количество < _currentКниги_в_продаже.Количество)
                    {
                        MessageBox.Show("На складе нет столько книг");
                        return;
                    }
                }

                _currentКниги_в_продаже.К_оплате = _currentКниги_в_продаже.Количество * book.Цена;
                context.SaveChanges();
                Manager.Forma.Navigate(new Rent());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EditStatus(object sender, RoutedEventArgs e)
        {
            var context = Book_houseEntities.GetContext();

            if (_currentКниги_в_продаже.id == 0)
            {
                MessageBox.Show("Нельзя изменить статус, пока книга не перешла в аренду");
                return;
            }

            if (_currentКниги_в_продаже.Количество <= 0)
            {
                MessageBox.Show("Количество не может быть меньше или равно 0");
                return;
            }

            var book = context.Книги.FirstOrDefault(d => d.id == _currentКниги_в_продаже.id_Книги);
            if (book == null)
            {
                MessageBox.Show("Книга не найдена в базе");
                return;
            }

            try
            {
                if (_currentКниги_в_продаже.Статус == 1)
                {
                    _currentКниги_в_продаже.Статус = 2;
                    book.Количество += _currentКниги_в_продаже.Количество;
                    Статус.Text = "Книга возвращена";
                    MessageBox.Show("Статус изменён на Книга возвращена");
                }
                else
                {
                    if (book.Количество < _currentКниги_в_продаже.Количество)
                    {
                        MessageBox.Show("На складе нет столько книг для выставления в аренду");
                        return;
                    }

                    _currentКниги_в_продаже.Статус = 1;
                    book.Количество -= _currentКниги_в_продаже.Количество;
                    Статус.Text = "В аренде";
                    MessageBox.Show("Статус изменён на В аренде");
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
