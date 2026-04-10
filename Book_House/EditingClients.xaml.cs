using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Book_House.Logging;
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
    /// Логика взаимодействия для EditingClients.xaml
    /// </summary>
    public partial class EditingClients : Page
    {
        public Клиенты _currentКлиенты = new Клиенты();
        public EditingClients(Клиенты selectedКлиенты)
        {
            InitializeComponent();
            if (selectedКлиенты != null)
                _currentКлиенты = selectedКлиенты;

            DataContext = _currentКлиенты;
            try
            {
                AppLogger.Info($"Открытие формы редактирования клиента. Id={_currentКлиенты?.id}");
            }
            catch { }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();

            // basic validation
            if (string.IsNullOrWhiteSpace(Фамилия.Text))
                errors.AppendLine("Укажите фамилию");
            if (string.IsNullOrWhiteSpace(Имя.Text))
                errors.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(Адрес.Text))
                errors.AppendLine("Укажите адрес");
            if (string.IsNullOrWhiteSpace(Телефон.Text))
                errors.AppendLine("Укажите телефон");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // normalize phone: extract digits
            var digits = string.Join(string.Empty, Regex.Matches(Телефон.Text ?? string.Empty, "\\d+").OfType<Match>().Select(m => m.Value));
            if (digits.Length != 11)
                errors.AppendLine("Проверьте кол-во цифр в номере");
            else
            {
                if (digits[0] == '7')
                    _currentКлиенты.Телефон = Regex.Replace(digits, "(\\d{1})(\\d{3})(\\d{0,3})(\\d{0,2})(\\d{0,2})", "+$1($2)$3-$4-$5");
                else if (digits[0] == '8')
                    _currentКлиенты.Телефон = Regex.Replace(digits, "(\\d{1})(\\d{3})(\\d{0,3})(\\d{0,2})(\\d{0,2})", "$1($2)$3-$4-$5");
                else
                    errors.AppendLine("Номер должен начинаться с 7 или 8");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(Отчество.Text))
                    _currentКлиенты.Отчество = "Нет";

                var context = Book_houseEntities.GetContext();
                if (_currentКлиенты.id == 0)
                    context.Клиенты.Add(_currentКлиенты);

                context.SaveChanges();
                AppLogger.Info($"Клиент сохранён. Id={_currentКлиенты.id}, ФИО={_currentКлиенты.Фамилия} {_currentКлиенты.Имя}");
                Manager.Forma.Navigate(new Клиенты1());
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка сохранения клиента: " + ex);
                MessageBox.Show("Произошла ошибка при сохранении. Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
