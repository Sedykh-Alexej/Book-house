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
    /// Логика взаимодействия для Postav.xaml
    /// </summary>
    public partial class Postav : Page
    {
        public Postav()
        {
            InitializeComponent();
            try
            {
                Поставщики.ItemsSource = Book_houseEntities.GetContext().Поставщики.ToList();
                Книги.ItemsSource = Book_houseEntities.GetContext().Книги.ToList();
                AppLogger.Info("Открыта страница Поставки");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при загрузке данных на странице Поставки: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при загрузке данных. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAdd_click(object sender, RoutedEventArgs e)
        {
            AppLogger.Info("Переход на страницу добавления поставки");
            Manager.Forma.Navigate(new EditPostav());
        }



        private void BtnDelete_click(object sender, RoutedEventArgs e)
        {
            var PostForRemoving = DGridPost.SelectedItems.Cast<Поставки>().ToList();
            if (!PostForRemoving.Any())
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Выберите элементы для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var confirm = Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), $"Вы точно хотите удалить следующее {PostForRemoving.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                Book_houseEntities.GetContext().Поставки.RemoveRange(PostForRemoving);
                Book_houseEntities.GetContext().SaveChanges();
                AppLogger.Info($"Удалено записей поставок: {PostForRemoving.Count}");
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Данные удалены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DGridPost.ItemsSource = Book_houseEntities.GetContext().Поставки.ToList();
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при удалении поставок: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при удалении. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Chief());
        }

        private void Все(object sender, RoutedEventArgs e)
        {
            try
            {
                DGridPost.ItemsSource = Book_houseEntities.GetContext().Поставки.ToList();
                AppLogger.Info("Загружены все поставки");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при загрузке всех поставок: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при загрузке данных. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Обновить(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Поставщики.Text))
                errors.AppendLine("Укажите поставщика");

            if (errors.Length > 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                AppLogger.Warn($"Валидация фильтра поставщиков не пройдена: {errors}");
                return;
            }

            try
            {
                var ctx = Book_houseEntities.GetContext();
                var Поставщик = ctx.Поставщики.FirstOrDefault(d => d.Наименование == Поставщики.Text);
                if (Поставщик == null)
                {
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Поставщик не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    AppLogger.Warn($"Поставщик для фильтра не найден: {Поставщики.Text}");
                    return;
                }
                DGridPost.ItemsSource = ctx.Поставки.Where(d => d.id_Поставщика == Поставщик.id).ToList();
                AppLogger.Info($"Фильтр поставок по поставщику: {Поставщик.Наименование}");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при фильтрации поставок: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при фильтрации. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Обновить2(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Книги.Text))
                errors.AppendLine("Укажите книгу");

            if (errors.Length > 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                AppLogger.Warn($"Валидация фильтра книги не пройдена: {errors}");
                return;
            }

            try
            {
                var ctx = Book_houseEntities.GetContext();
                var Книга = ctx.Книги.FirstOrDefault(d => d.Название == Книги.Text);
                if (Книга == null)
                {
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Книга не найдена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    AppLogger.Warn($"Книга для фильтра не найдена: {Книги.Text}");
                    return;
                }
                DGridPost.ItemsSource = ctx.Поставки.Where(d => d.id_Книги == Книга.id).ToList();
                AppLogger.Info($"Фильтр поставок по книге: {Книга.Название}");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при фильтрации поставок по книге: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при фильтрации. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
