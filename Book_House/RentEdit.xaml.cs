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
            КлиентИмя.ItemsSource = Book_houseEntities.GetContext().Клиенты.ToList();
            КлиентФамилия.ItemsSource = Book_houseEntities.GetContext().Клиенты.ToList();
            КлиентОтчество.ItemsSource = Book_houseEntities.GetContext().Клиенты.ToList();
            Книга.ItemsSource = Book_houseEntities.GetContext().Книги.ToList();
            Сотрудник.Text = Manager.IFO;
            if (_currentКниги_в_продаже.Статус == 0)
            {
                Статус.Text = "В аренде";
            }
            else
            {
                Статус.Text = _currentКниги_в_продаже.Статус1.Название;
            }
        }
        
private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Книга.Text))
                errors.AppendLine("Укажите название книги");
            if (_currentКниги_в_продаже.id_Клиента <=0)
                errors.AppendLine("Укажите клиента");
            if (string.IsNullOrWhiteSpace(Количество.Text))
                errors.AppendLine("Укажите количество");
            if (_currentКниги_в_продаже.Количество <= 0)
                errors.AppendLine("Количество не может быть меньше или равно 0");



            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentКниги_в_продаже.id == 0)
            {
                _currentКниги_в_продаже.id_Сотрудника = Manager.IDSotr;
                _currentКниги_в_продаже.Дата_продажи = DateTime.Today;
                _currentКниги_в_продаже.Статус = 1;
                Book_houseEntities.GetContext().Продажа_книг.Add(_currentКниги_в_продаже);
                var Книгаа = Book_houseEntities.GetContext().Книги.Where(d => d.id == _currentКниги_в_продаже.id_Книги).FirstOrDefault();
                Книгаа.Количество -= _currentКниги_в_продаже.Количество;
            }


            try
            {
                var Книгаа = Book_houseEntities.GetContext().Книги.Where(d => d.id == _currentКниги_в_продаже.id_Книги).FirstOrDefault();
                long Кол_во = Convert.ToInt64(Количество.Text);
                if (Книгаа.Количество < Кол_во)
                    errors.AppendLine("На складе нет столько книг");
                if (errors.Length > 0)
                    {
                        MessageBox.Show(errors.ToString());
                        return;
                    }
                _currentКниги_в_продаже.К_оплате = _currentКниги_в_продаже.Количество * Книгаа.Цена;
                Book_houseEntities.GetContext().SaveChanges();
                Manager.Forma.Navigate(new Rent());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        
        private void EditStatus(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (_currentКниги_в_продаже.Количество <= 0)
                errors.AppendLine("Количество не может быть меньше или равно 0");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }


            if (_currentКниги_в_продаже.Статус == 1)
                {
                if (_currentКниги_в_продаже.id != 0)
                {
                    _currentКниги_в_продаже.Статус = 2;
                    var Книгаа = Book_houseEntities.GetContext().Книги.Where(d => d.id == _currentКниги_в_продаже.id_Книги).FirstOrDefault();
                    Книгаа.Количество += _currentКниги_в_продаже.Количество;
                    MessageBox.Show("Статус изменён на Книга возвращена");
                    Статус.Text = "Книга возвращена";
                }
                else
                {
                    MessageBox.Show("Нельзя изменить статус, пока книга не перешла в аренду");
                }
            }
                else
                {
                if (_currentКниги_в_продаже.id != 0)
                {
                    _currentКниги_в_продаже.Статус = 1;
                    var Книгаа = Book_houseEntities.GetContext().Книги.Where(d => d.id == _currentКниги_в_продаже.id_Книги).FirstOrDefault();
                    Книгаа.Количество -= _currentКниги_в_продаже.Количество;
                    MessageBox.Show("Статус изменён на В аренде");
                    Статус.Text = "В аренде";
                }
                else
                {
                    MessageBox.Show("Нельзя изменить статус, пока книга не перешла в аренду");
                }
            }
            
        }
    }
}
