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
using Book_House.Logging;

namespace Book_House
{
    /// <summary>
    /// Логика взаимодействия для Edit_Write_offs.xaml
    /// </summary>
    public partial class Edit_Write_offs : Page
    {
        public Edit_Write_offs()
        {
            InitializeComponent();
            id_сотрудника.Text = Manager.IFO;
            try
            {
                Название.ItemsSource = Book_houseEntities.GetContext().Книги.ToList();
                AppLogger.Info("Открыта страница добавления списания");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при загрузке данных на странице добавления списания: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при загрузке данных. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Название.Text))
                errors.AppendLine("Укажите название книги");
            if (string.IsNullOrWhiteSpace(Количество.Text))
                errors.AppendLine("Укажите количество");
            if (string.IsNullOrWhiteSpace(Причина.Text))
                errors.AppendLine("Укажите причину");

            if (errors.Length > 0)
            {
                AppLogger.Warn("Валидация при добавлении списания: " + errors.ToString());
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(Количество.Text, out int Кол_во))
            {
                AppLogger.Warn($"Неверный формат поля 'Количество': '{Количество.Text}'");
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Количество должно быть целым числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Кол_во <= 0)
            {
                AppLogger.Warn($"Недопустимое значение количества: {Кол_во}");
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Количество должно быть больше 0.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var Книга = Book_houseEntities.GetContext().Книги.Where(d => d.Название == Название.Text).FirstOrDefault();
            if (Книга == null)
            {
                AppLogger.Warn($"Попытка списания несуществующей книги: '{Название.Text}'");
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Книга не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int current = Книга.Количество ?? 0;
            if (Кол_во > current)
            {
                AppLogger.Warn($"Попытка списания {Кол_во} шт., доступно: {current} шт. Книга: '{Книга.Название}'");
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Недостаточно экземпляров для списания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Книга.Количество = current - Кол_во;
            Списание_книг списание = new Списание_книг(0, Книга.id, Кол_во, Причина.Text, Manager.IDSotr);
            Book_houseEntities.GetContext().Списание_книг.Add(списание);

            try
            {
                Book_houseEntities.GetContext().SaveChanges();
                AppLogger.Info($"Создано списание книги id={Книга.id}, Название='{Книга.Название}', Количество={Кол_во}, сотрудник id={Manager.IDSotr}");
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Списание успешно сохранено.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.Forma.Navigate(new Write_offs());
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при сохранении списания: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при сохранении. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Write_offs());
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

    }
}
