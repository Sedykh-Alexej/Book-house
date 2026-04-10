using Book_House.Logging;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
            var context = Book_houseEntities.GetContext();
            var клиенты = context.Клиенты.ToList();
            КлиентИмя.ItemsSource = клиенты;
            КлиентФамилия.ItemsSource = клиенты;
            КлиентОтчество.ItemsSource = клиенты;
            Книга.ItemsSource = context.Книги.ToList();
            Сотрудник.Text = Manager.IFO;

            Статус.Text = _currentКниги_в_продаже.Статус == 0
                ? "В продаже"
                : _currentКниги_в_продаже.Статус1?.Название ?? string.Empty;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            var context = Book_houseEntities.GetContext();

            if (string.IsNullOrWhiteSpace(Книга.Text))
                errors.AppendLine("Укажите название книги");
            if (_currentКниги_в_продаже.id_Клиента <= 0)
                errors.AppendLine("Укажите клиента");

            if (string.IsNullOrWhiteSpace(Количество.Text))
                errors.AppendLine("Укажите количество");

            if (!long.TryParse(Количество.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out long parsedКол))
                errors.AppendLine("Количество должно быть целым числом");

            if (parsedКол <= 0)
                errors.AppendLine("Количество не может быть меньше или равно 0");

            if (errors.Length > 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _currentКниги_в_продаже.Количество = (int)parsedКол;

            try
            {
                var book = context.Книги.FirstOrDefault(d => d.id == _currentКниги_в_продаже.id_Книги);
                if (book == null)
                {
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Выбранная книга не найдена в базе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_currentКниги_в_продаже.id == 0)
                {
                    _currentКниги_в_продаже.id_Сотрудника = Manager.IDSotr;
                    _currentКниги_в_продаже.Дата_продажи = DateTime.Today;
                    _currentКниги_в_продаже.Статус = 1;
                    context.Продажа_книг.Add(_currentКниги_в_продаже);

                    if (book.Количество < _currentКниги_в_продаже.Количество)
                    {
                        Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "На складе нет столько книг", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    book.Количество -= _currentКниги_в_продаже.Количество;
                }
                else
                {
                    if (book.Количество < _currentКниги_в_продаже.Количество)
                    {
                        Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "На складе нет столько книг", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                _currentКниги_в_продаже.К_оплате = _currentКниги_в_продаже.Количество * book.Цена;
                context.SaveChanges();
                AppLogger.Info($"Сохранена продажа Id={_currentКниги_в_продаже.id}");
                Manager.Forma.Navigate(new Rent());
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка сохранения продажи: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при сохранении. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditStatus(object sender, RoutedEventArgs e)
        {
            var context = Book_houseEntities.GetContext();

            if (_currentКниги_в_продаже.id == 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Нельзя изменить статус, пока продажа не совершена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentКниги_в_продаже.Количество <= 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Количество не может быть меньше или равно 0", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var book = context.Книги.FirstOrDefault(d => d.id == _currentКниги_в_продаже.id_Книги);
            if (book == null)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Книга не найдена в базе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (_currentКниги_в_продаже.Статус == 2)
                {
                    if (book.Количество < _currentКниги_в_продаже.Количество)
                    {
                        Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "На складе нет столько книг для выставления в продажу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    _currentКниги_в_продаже.Статус = 1;
                    book.Количество -= _currentКниги_в_продаже.Количество;
                    Статус.Text = "В продаже";
                    AppLogger.Info($"Статус продажи Id={_currentКниги_в_продаже.id} изменён на 'В продаже'");
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Статус будет изменён на 'В продаже', после сохранения", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (_currentКниги_в_продаже.Статус == 1)
                {
                    _currentКниги_в_продаже.Статус = 2;
                    book.Количество += _currentКниги_в_продаже.Количество;

                    _currentКниги_в_продаже.Дата_продажи = DateTime.Today;

                    Статус.Text = "Книга возвращена";
                    AppLogger.Info($"Статус продажи Id={_currentКниги_в_продаже.id} изменён на 'Книга возвращена'");
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Статус будет изменён на 'Книга возвращена', после сохранения", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Неподдерживаемый статус для изменения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка изменения статуса продажи: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Произошла ошибка при изменении статуса. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            try
            {
                var ctx = Book_houseEntities.GetContext();
                var entry = ctx.Entry(_currentКниги_в_продаже);
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
                AppLogger.Error("Ошибка при отмене изменений продажи: " + ex);
            }

            Manager.Forma.Navigate(new Rent());
        }

        private void Количество_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void Количество_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
                e.Handled = true;
        }

        private void Количество_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var text = e.DataObject.GetData(DataFormats.Text) as string;
                if (!text.All(char.IsDigit))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
