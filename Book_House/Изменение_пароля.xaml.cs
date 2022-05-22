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
    /// Логика взаимодействия для Изменение_пароля.xaml
    /// </summary>
    public partial class Изменение_пароля : Page
    {
        public Изменение_пароля()
        {
            InitializeComponent();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Manager.Forma.GoBack();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(PasswordBoxx.Password))
                errors.AppendLine("Укажите пароль пожалуйста");
            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                errors.AppendLine("Повторите пароль пожалуйста");
            if (PasswordBoxx.Password != PasswordBox.Password)
                errors.AppendLine("Пароли не совпадают");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                var Сотрудник = Book_houseEntities.GetContext().Сотрудники.Where(d => (d.id == Manager.IDSotr)).FirstOrDefault();
                Сотрудник.Пароль = PasswordBoxx.Password;
                Book_houseEntities.GetContext().SaveChanges();
                MessageBox.Show("Пароль обновлён!");
                Manager.Forma.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
