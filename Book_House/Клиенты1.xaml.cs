using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Book_House.Logging;

namespace Book_House
{
    /// <summary>
    /// Логика взаимодействия для Клиенты1.xaml
    /// </summary>
    public partial class Клиенты1 : Page
    {
        public Клиенты1()
        {
            InitializeComponent();
            try
            {
                var list = Book_houseEntities.GetContext().Клиенты.ToList();
                Фамилия.ItemsSource = list;
                Имя.ItemsSource = list;
                Отчество.ItemsSource = list;
                AppLogger.Info($"Открыт список клиентов, всего: {list.Count}");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Не удалось загрузить список клиентов: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Не удалось загрузить список клиентов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEdit_click(object sender, RoutedEventArgs e)
        {
            var client = (sender as Button)?.DataContext as Клиенты;
            if (client == null)
            {
                AppLogger.Info("Попытка редактирования: выбранный элемент пуст");
                return;
            }
            AppLogger.Info($"Переход к редактированию клиента Id={client.id}");
            Manager.Forma.Navigate(new EditingClients(client));
        }

        private void BtnAdd_click(object sender, RoutedEventArgs e)
        {
            AppLogger.Info("Переход к добавлению нового клиента");
            Manager.Forma.Navigate(new EditingClients(null));
        }

        private void BtnDelete_click(object sender, RoutedEventArgs e)
        {
            var selected = DGridClient.SelectedItems.Cast<Клиенты>().ToList();
            if (selected.Count == 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Выберите элементы для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var confirm = Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), $"Вы точно хотите удалить следующее {selected.Count} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) 
            {
                AppLogger.Info("Удаление клиентов отменено пользователем");
                return;
            }

            try
            {
                var ctx = Book_houseEntities.GetContext();
                ctx.Клиенты.RemoveRange(selected);
                ctx.SaveChanges();
                AppLogger.Info($"Удалено клиентов: {selected.Count}");
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Данные удалены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DGridClient.ItemsSource = Book_houseEntities.GetContext().Клиенты.ToList();
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при удалении клиентов: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при удалении. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            AppLogger.Info("Возврат в форму кассира");
            Manager.Forma.Navigate(new Сashier());
        }

        private void Все(object sender, RoutedEventArgs e)
        {
            try
            {
                DGridClient.ItemsSource = Book_houseEntities.GetContext().Клиенты.ToList();
                AppLogger.Info("Показаны все клиенты");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка загрузки клиентов: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Не удалось получить список клиентов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Фамилия.Text))
                errors.AppendLine("Укажите Фамилию");
            if (string.IsNullOrWhiteSpace(Имя.Text))
                errors.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(Отчество.Text))
                errors.AppendLine("Укажите отчество");

            if (errors.Length > 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var ctx = Book_houseEntities.GetContext();
                var client = ctx.Клиенты.FirstOrDefault(d => d.Фамилия == Фамилия.Text && d.Имя == Имя.Text && d.Отчество == Отчество.Text);
                if (client != null)
                {
                    DGridClient.ItemsSource = ctx.Клиенты.Where(d => d.id == client.id).ToList();
                    AppLogger.Info($"Поиск клиента: Id={client.id}");
                }
                else
                {
                    AppLogger.Info($"Клиент не найден: {Фамилия.Text} {Имя.Text} {Отчество.Text}");
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Не существует клиента с данным именем, фамилией и отчеством", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при поиске клиента: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Произошла ошибка при поиске. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
