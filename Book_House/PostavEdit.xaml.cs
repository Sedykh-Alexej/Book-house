using Book_House.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            AppLogger.Info($"Открыта страница PostavEdit для поставщика Id={_currentПоставщики.id}");
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {


            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Наименование.Text))
                errors.AppendLine("Укажите наименование");
            if (string.IsNullOrWhiteSpace(Адрес.Text))
                errors.AppendLine("Укажите адрес");
            if (string.IsNullOrWhiteSpace(Телефон.Text))
                errors.AppendLine("Укажите телефон");

            if (errors.Length > 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                AppLogger.Warn($"Валидация поставщика не пройдена: {errors}");
                return;
            }

            var ctx = Book_houseEntities.GetContext();
            if (_currentПоставщики.id == 0)
            {
                ctx.Поставщики.Add(_currentПоставщики);
                AppLogger.Info("Новый поставщик добавлен в контекст");
            }

            string resultString = string.Join(string.Empty, Regex.Matches(Телефон.Text, "\\d+").OfType<Match>().Select(m => m.Value));
            if (resultString.Length != 11)
                errors.AppendLine("Проверьте кол-во цифр в номере");
            if (resultString.Length == 0)
                errors.AppendLine("Номер телефона не содержит цифр");

            if (errors.Length > 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                AppLogger.Warn($"Проверка телефона не пройдена: {errors}");
                return;
            }

            if (resultString[0] == '7')
            {
                _currentПоставщики.Телефон = Regex.Replace(resultString, "(\\d{1})(\\d{3})(\\d{0,3})(\\d{0,2})(\\d{0,2})", "+$1($2)$3-$4-$5");
            }
            else if (resultString[0] == '8')
            {
                _currentПоставщики.Телефон = Regex.Replace(resultString, "(\\d{1})(\\d{3})(\\d{0,3})(\\d{0,2})(\\d{0,2})", "$1($2)$3-$4-$5");
            }
            else
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Номер должен начинаться с +7 или 8", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                AppLogger.Warn($"Номер телефона не соответствует формату: {Телефон.Text}");
                return;
            }

            try
            {
                try
                {
                    ctx.SaveChanges();
                    AppLogger.Info($"Поставщик сохранён Id={_currentПоставщики.id}");
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Данные поставщика сохранены", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    Manager.Forma.Navigate(new Post());
                }
                catch (Exception ex)
                {
                    AppLogger.Error("Ошибка при сохранении поставщика: " + ex);
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при сохранении поставщика. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при сохранении поставщика: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при сохранении поставщика. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            try
            {
                var ctx = Book_houseEntities.GetContext();
                var entry = ctx.Entry(_currentПоставщики);
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
                AppLogger.Error("Ошибка при отмене изменений поставщика: " + ex);
            }

            Manager.Forma.Navigate(new Rent());
        }
    }
}
