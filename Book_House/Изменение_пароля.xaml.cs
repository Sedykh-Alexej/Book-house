using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Book_House.Logging;
using Book_House.Security;

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

            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(PasswordBoxx.Password))
                errors.AppendLine("Укажите пароль пожалуйста");
            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                errors.AppendLine("Повторите пароль пожалуйста");
            if (PasswordBoxx.Password != PasswordBox.Password)
                errors.AppendLine("Пароли не совпадают");

            if (errors.Length > 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var ctx = Book_houseEntities.GetContext();
                var employee = ctx.Сотрудники.FirstOrDefault(d => d.id == Manager.IDSotr);
                if (employee == null)
                {
                    AppLogger.Error($"Сотрудник с Id={Manager.IDSotr} не найден при смене пароля");
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (PasswordHelper.VerifyPassword(employee.Пароль, PasswordBoxx.Password))
                {
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Пароль совпадает с предыдущим", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                employee.Пароль = PasswordHelper.HashPassword(PasswordBoxx.Password);
                ctx.SaveChanges();
                AppLogger.Info($"Сотрудник Id={employee.id} обновил пароль");
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Пароль обновлён!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.Forma.GoBack();
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при смене пароля: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при смене пароля. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
