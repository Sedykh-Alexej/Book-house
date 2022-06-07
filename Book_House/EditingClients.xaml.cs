using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Фамилия.Text))
                errors.AppendLine("Укажите фамилию");
            if (string.IsNullOrWhiteSpace(Имя.Text))
                errors.AppendLine("Укажите Имя");
            if (string.IsNullOrWhiteSpace(Адрес.Text))
                errors.AppendLine("Укажите Адрес");
            if (string.IsNullOrWhiteSpace(Телефон.Text))
                errors.AppendLine("Укажите Телефон");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentКлиенты.id == 0)
            {
                Book_houseEntities.GetContext().Клиенты.Add(_currentКлиенты);
            }


            string resultString = string.Join(string.Empty, Regex.Matches(Телефон.Text, @"\d+").OfType<Match>().Select(m => m.Value));
            if (resultString.Length != 11)
                errors.AppendLine("Проверьте кол-во цифр в номере");
            if (resultString[0] == '7')
            {
                _currentКлиенты.Телефон = Regex.Replace(resultString, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})(\d{0,2})", "+$1($2)$3-$4-$5");
            }
            else if (resultString[0] == '8')
            {
                _currentКлиенты.Телефон = Regex.Replace(resultString, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})(\d{0,2})", "$1($2)$3-$4-$5");
            }
            else
            {
                errors.AppendLine("Номер должен начинаться с +7 или 8");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(Отчество.Text))
                    _currentКлиенты.Отчество = "Нет";

                Book_houseEntities.GetContext().SaveChanges();
                Manager.Forma.Navigate(new Клиенты1());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
