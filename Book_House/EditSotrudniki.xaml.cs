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

            try
            {
                try
                {
                    long Phone = Convert.ToInt64(Телефон.Text);
                    _currentСотрудники.Телефон = Phone.ToString("+#-###-###-##-##");
                }
                catch (Exception) { }

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
