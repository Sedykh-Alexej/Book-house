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
    /// Логика взаимодействия для PostavEdit.xaml
    /// </summary>
    public partial class PostavEdit : Page
    {
        public Поставщики _currentПоставщики = new Поставщики();

        public PostavEdit(Поставщики selectedПоставщики)
        {
            InitializeComponent();
            if (selectedПоставщики != null)
                _currentПоставщики = selectedПоставщики;

            DataContext = _currentПоставщики;
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {


            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Наименование.Text))
                errors.AppendLine("Укажите наименование");
            if (string.IsNullOrWhiteSpace(Адрес.Text))
                errors.AppendLine("Укажите адрес");
            if (string.IsNullOrWhiteSpace(Телефон.Text))
                errors.AppendLine("Укажите телефон");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentПоставщики.id == 0)
            {
                Book_houseEntities.GetContext().Поставщики.Add(_currentПоставщики);
            }

            string resultString = string.Join(string.Empty, Regex.Matches(Телефон.Text, @"\d+").OfType<Match>().Select(m => m.Value));
            if (resultString.Length != 11)
                errors.AppendLine("Проверьте кол-во цифр в номере");
            if (resultString[0] == '7')
            {
                _currentПоставщики.Телефон = Regex.Replace(resultString, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})(\d{0,2})", "+$1($2)$3-$4-$5");
            }
            else if (resultString[0] == '8')
            {
                _currentПоставщики.Телефон = Regex.Replace(resultString, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})(\d{0,2})", "$1($2)$3-$4-$5");
            }
            else
            {
                errors.AppendLine("Номер должен начинаться с +7 или 8");
            }

            try
            {
                Book_houseEntities.GetContext().SaveChanges();
                Manager.Forma.Navigate(new Post());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
