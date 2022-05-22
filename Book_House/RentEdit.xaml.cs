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
        public Книги_в_аренде _currentКниги_в_аренде = new Книги_в_аренде();
        public RentEdit(Книги_в_аренде selectedКниги_в_аренде)
        {
            InitializeComponent();
            if (selectedКниги_в_аренде != null)
                _currentКниги_в_аренде = selectedКниги_в_аренде;

            DataContext = _currentКниги_в_аренде;
            КлиентИмя.ItemsSource = Book_houseEntities.GetContext().Клиенты.ToList();
            КлиентФамилия.ItemsSource = Book_houseEntities.GetContext().Клиенты.ToList();
            КлиентОтчество.ItemsSource = Book_houseEntities.GetContext().Клиенты.ToList();
            Статус.ItemsSource = Book_houseEntities.GetContext().Статус.ToList();
            Книга.ItemsSource = Book_houseEntities.GetContext().Книги.ToList();
            Сотрудник.Text = Manager.IFO;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Книга.Text))
                errors.AppendLine("Укажите название книги");
            if (string.IsNullOrWhiteSpace(Статус.Text))
                errors.AppendLine("Укажите статус");
            if (string.IsNullOrWhiteSpace(Дата_Возврата.Text))
                errors.AppendLine("Укажите дату возврата");
            if (_currentКниги_в_аренде.id_Клиента <=0)
                errors.AppendLine("Укажите клиента");



            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentКниги_в_аренде.id == 0)
            {
                _currentКниги_в_аренде.id_Сотрудника = Manager.IDSotr;
                _currentКниги_в_аренде.Дата_получения = DateTime.Today;
                Book_houseEntities.GetContext().Книги_в_аренде.Add(_currentКниги_в_аренде);
            }

            try
            {
                Book_houseEntities.GetContext().SaveChanges();
                Manager.Forma.Navigate(new Rent());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
