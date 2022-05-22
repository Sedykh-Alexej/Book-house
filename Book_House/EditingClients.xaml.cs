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
            if (string.IsNullOrWhiteSpace(Отчество.Text))
                errors.AppendLine("Укажите Отчество");
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

            try
            {
                Book_houseEntities.GetContext().SaveChanges();
                Manager.Forma.Navigate(new Клиенты1());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Cashier());
        }
    }
}
