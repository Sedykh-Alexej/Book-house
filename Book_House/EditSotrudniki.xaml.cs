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
    /// Логика взаимодействия для EditSotrudniki.xaml
    /// </summary>
    public partial class EditSotrudniki : Page
    {
        public Сотрудники _currentСотрудники = new Сотрудники();
        public EditSotrudniki(Сотрудники selectedСотрудники)
        {
            InitializeComponent();
            if (selectedСотрудники != null)
                _currentСотрудники = selectedСотрудники;

            DataContext = _currentСотрудники;
            Должность.ItemsSource = Book_houseEntities.GetContext().Должности.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Имя.Text))
                errors.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(Фамилия.Text))
                errors.AppendLine("Укажите фамилию");
            if (string.IsNullOrWhiteSpace(Адрес.Text))
                errors.AppendLine("Укажите адрес");
            if (string.IsNullOrWhiteSpace(Телефон.Text))
                errors.AppendLine("Укажите телефон");
            if (string.IsNullOrWhiteSpace(График.Text))
                errors.AppendLine("Укажите график");
            if (string.IsNullOrWhiteSpace(Должность.Text))
                errors.AppendLine("Укажите должность");



            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentСотрудники.id == 0)
            {
                _currentСотрудники.Пароль = "1234";
                _currentСотрудники.Стиль = "Second_style.xaml";
                Book_houseEntities.GetContext().Сотрудники.Add(_currentСотрудники);
            }



            string resultString = string.Join(string.Empty, Regex.Matches(Телефон.Text, @"\d+").OfType<Match>().Select(m => m.Value));
            if (resultString.Length != 11)
                errors.AppendLine("Проверьте кол-во цифр в номере");
            if (resultString[0] == '7')
            {
                _currentСотрудники.Телефон = Regex.Replace(resultString, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})(\d{0,2})", "+$1($2)$3-$4-$5");
            }
            else if (resultString[0] == '8')
            {
                _currentСотрудники.Телефон = Regex.Replace(resultString, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})(\d{0,2})", "$1($2)$3-$4-$5");
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
                    _currentСотрудники.Отчество = "Нет";
                Book_houseEntities.GetContext().SaveChanges();
                Manager.Forma.Navigate(new Sotrudniki());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
