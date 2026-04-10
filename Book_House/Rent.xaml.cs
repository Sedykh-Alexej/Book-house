using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Book_House.Logging;

namespace Book_House
{
    /// <summary>
    /// Логика взаимодействия для Rent.xaml
    /// </summary>
    public partial class Rent : Page
    {
        public Rent()
        {
            InitializeComponent();
            try
            {
                var list = Book_houseEntities.GetContext().Продажа_книг.ToList();
                DGridRent.ItemsSource = list;
                AppLogger.Info($"Открыт список продаж, всего: {list.Count}");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Не удалось загрузить список продаж: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Не удалось загрузить список продаж.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEdit_click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button)?.DataContext as Продажа_книг;
            if (item == null)
            {
                AppLogger.Info("Попытка редактирования: выбранный элемент пуст");
                return;
            }
            AppLogger.Info($"Переход к редактированию продажи Id={item.id}");
            Manager.Forma.Navigate(new RentEdit(item));
        }

        private void BtnAdd_click(object sender, RoutedEventArgs e)
        {
            AppLogger.Info("Переход к добавлению новой продажи");
            Manager.Forma.Navigate(new RentEdit(null));
        }

        private void BtnDelete_click(object sender, RoutedEventArgs e)
        {
            var selected = DGridRent.SelectedItems.Cast<Продажа_книг>().ToList();
            if (selected.Count == 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Выберите элементы для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var confirm = Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), $"Вы точно хотите удалить следующее {selected.Count} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes)
            {
                AppLogger.Info("Удаление продаж отменено пользователем");
                return;
            }

            try
            {
                var ctx = Book_houseEntities.GetContext();
                ctx.Продажа_книг.RemoveRange(selected);
                ctx.SaveChanges();
                AppLogger.Info($"Удалено продаж: {selected.Count}");
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Данные удалены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DGridRent.ItemsSource = Book_houseEntities.GetContext().Продажа_книг.ToList();
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при удалении продаж: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при удалении. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            AppLogger.Info("Возврат в форму кассира из Rent");
            Manager.Forma.Navigate(new Сashier());
        }
    }
}
