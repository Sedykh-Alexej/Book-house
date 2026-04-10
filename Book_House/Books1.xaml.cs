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
    /// Логика взаимодействия для Books1.xaml
    /// </summary>
    public partial class Books1 : Page
    {
        public Books1()
        {
            InitializeComponent();
            try
            {
                Жанр.ItemsSource = Book_houseEntities.GetContext().Жанры.ToList();
                Автор.ItemsSource = Book_houseEntities.GetContext().Авторы.ToList();
                AppLogger.Info("Открыта страница Книги");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при загрузке данных на странице Книги: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при загрузке данных. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void BtnEdit_click(object sender, RoutedEventArgs e)
        {
            AppLogger.Info("Переход на страницу редактирования книги");
            Manager.Forma.Navigate(new BookEdit((sender as Button).DataContext as Книги));
        }

        private void BtnAdd_click(object sender, RoutedEventArgs e)
        {
            AppLogger.Info("Переход на страницу добавления книги");
            Manager.Forma.Navigate(new BookEdit(null));
        }

        private void BtnDelete_click(object sender, RoutedEventArgs e)
        {
            var PostavForRemoving = DGridBook.SelectedItems.Cast<Книги>().ToList();
            if (!PostavForRemoving.Any())
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Выберите элементы для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var confirm = Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), $"Вы точно хотите удалить следующее {PostavForRemoving.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                Book_houseEntities.GetContext().Книги.RemoveRange(PostavForRemoving);
                Book_houseEntities.GetContext().SaveChanges();
                AppLogger.Info($"Удалено книг: {PostavForRemoving.Count}");
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Данные удалены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DGridBook.ItemsSource = Book_houseEntities.GetContext().Книги.ToList();
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при удалении книг: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при удалении. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Chief());
        }

        private void Обновить(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Жанр.Text))
                errors.AppendLine("Укажите жанр");

            if (errors.Length > 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                AppLogger.Warn($"Валидация фильтра жанра не пройдена: {errors}");
                return;
            }

            try
            {
                var ctx = Book_houseEntities.GetContext();
                var Жанрр = ctx.Жанры.FirstOrDefault(d => d.Наименование == Жанр.Text);
                if (Жанрр == null)
                {
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Жанр не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    AppLogger.Warn($"Жанр для фильтра не найден: {Жанр.Text}");
                    return;
                }
                DGridBook.ItemsSource = ctx.Книги.Where(d => d.Жанр == Жанрр.id).ToList();
                AppLogger.Info($"Фильтр книг по жанру: {Жанрр.Наименование}");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при фильтрации книг по жанру: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при фильтрации. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Обновить2(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Автор.Text))
                errors.AppendLine("Укажите автора");

            if (errors.Length > 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                AppLogger.Warn($"Валидация фильтра автора не пройдена: {errors}");
                return;
            }

            try
            {
                var ctx = Book_houseEntities.GetContext();
                var Avtor = ctx.Авторы.FirstOrDefault(d => d.Автор1 == Автор.Text);
                if (Avtor == null)
                {
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Автор не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    AppLogger.Warn($"Автор для фильтра не найден: {Автор.Text}");
                    return;
                }
                DGridBook.ItemsSource = ctx.Книги.Where(d => d.Автор == Avtor.id_Автора).ToList();
                AppLogger.Info($"Фильтр книг по автору: {Avtor.Автор1}");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при фильтрации книг по автору: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при фильтрации. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Все(object sender, RoutedEventArgs e)
        {
            try
            {
                DGridBook.ItemsSource = Book_houseEntities.GetContext().Книги.ToList();
                AppLogger.Info("Загружены все книги");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при загрузке всех книг: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при загрузке данных. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
