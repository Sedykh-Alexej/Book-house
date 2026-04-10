using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Book_House.Logging;

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
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentСотрудники.id == 0)
            {
                _currentСотрудники.Пароль = Book_House.Security.PasswordHelper.HashPassword("1234");
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
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            try
            {
                if (string.IsNullOrWhiteSpace(Отчество.Text))
                    _currentСотрудники.Отчество = "Нет";

                Book_houseEntities.GetContext().SaveChanges();
                AppLogger.Info($"Сотрудник сохранён. Id={_currentСотрудники.id}, ФИО={_currentСотрудники.Фамилия} {_currentСотрудники.Имя}");
                Manager.Forma.Navigate(new Sotrudniki());
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при сохранении сотрудника: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при сохранении. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            try
            {
                var ctx = Book_houseEntities.GetContext();
                var entry = ctx.Entry(_currentСотрудники);
                if (entry != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.State = EntityState.Detached;
                    }
                    else if (entry.State == EntityState.Modified)
                    {

                        try { entry.Reload(); } catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при отмене изменений сотрудника: " + ex);
            }

            Manager.Forma.Navigate(new Rent());
        }
    }
}
