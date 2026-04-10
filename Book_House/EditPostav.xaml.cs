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
    /// Логика взаимодействия для EditPostav.xaml
    /// </summary>
    public partial class EditPostav : Page
    {
        public EditPostav()
        {
            InitializeComponent();           
            id_сотрудника.Text = Manager.IFO;
            Book.ItemsSource = Book_houseEntities.GetContext().Книги.ToList();
            поставщик.ItemsSource = Book_houseEntities.GetContext().Поставщики.ToList();
            Book_House.Logging.AppLogger.Info($"Открыта страница EditPostav пользователем {Manager.IFO}");
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Book.Text))
                errors.AppendLine("Укажите название книги");
            if (string.IsNullOrWhiteSpace(Количество.Text))
                errors.AppendLine("Укажите количество");
            if (string.IsNullOrWhiteSpace(поставщик.Text))
                errors.AppendLine("Укажите поставщика");

            if (errors.Length > 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                Book_House.Logging.AppLogger.Warn($"Валидация при добавлении поставки не пройдена: {errors}");
                return;
            }

            if (!int.TryParse(Количество.Text, out int Кол_во))
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Количество должно быть числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                Book_House.Logging.AppLogger.Warn($"Неправильный формат количества: '{Количество.Text}'");
                return;
            }

            if (Кол_во <= 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Количество не может быть меньше или равно 0", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                Book_House.Logging.AppLogger.Warn($"Попытка добавить неверное количество: {Кол_во}");
                return;
            }

            try
            {
                var ctx = Book_houseEntities.GetContext();
                DateTime thisDay = DateTime.Today;
                var Книга = ctx.Книги.FirstOrDefault(d => d.Название == Book.Text);
                if (Книга == null)
                {
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Книга не найдена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Book_House.Logging.AppLogger.Error($"Книга с названием '{Book.Text}' не найдена при добавлении поставки");
                    return;
                }

                var Поставщик = ctx.Поставщики.FirstOrDefault(d => d.Наименование == поставщик.Text);
                if (Поставщик == null)
                {
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Поставщик не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Book_House.Logging.AppLogger.Error($"Поставщик с наименованием '{поставщик.Text}' не найден при добавлении поставки");
                    return;
                }

                Книга.Количество += Кол_во;
                Поставки поставка = new Поставки(0, Поставщик.id, Книга.id, Кол_во, thisDay, Manager.IDSotr);
                ctx.Поставки.Add(поставка);

                try
                {
                    ctx.SaveChanges();
                    Book_House.Logging.AppLogger.Info($"Добавлена поставка: КнигаId={Книга.id}, ПоставщикId={Поставщик.id}, Количество={Кол_во}, СотрудникId={Manager.IDSotr}");
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Поставка сохранена", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    Manager.Forma.Navigate(new Postav());
                }
                catch (Exception ex)
                {
                    Book_House.Logging.AppLogger.Error("Ошибка при сохранении поставки: " + ex);
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при сохранении поставки. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Book_House.Logging.AppLogger.Error("Ошибка в процессе подготовки поставки: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при обработке данных. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Количество_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void Количество_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Запрещаем пробел
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
        private void Exit(object sender, RoutedEventArgs e)
        {
            try
            {
                var ctx = Book_houseEntities.GetContext();
                var hasChanges = ctx.ChangeTracker.Entries().Any(entry => entry.State != System.Data.Entity.EntityState.Unchanged);
                if (hasChanges)
                {
                    var res = Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Есть несохранённые изменения. Отменить и выйти?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res != MessageBoxResult.Yes)
                        return;

                    foreach (var entry in ctx.ChangeTracker.Entries().ToList())
                    {
                        try
                        {
                            if (entry.State == System.Data.Entity.EntityState.Added)
                                entry.State = System.Data.Entity.EntityState.Detached;
                            else if (entry.State == System.Data.Entity.EntityState.Modified)
                                entry.Reload();
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Book_House.Logging.AppLogger.Error("Ошибка при выходе из EditPostav: " + ex);
            }

            Manager.Forma.Navigate(new Postav());
        }
    }
}
