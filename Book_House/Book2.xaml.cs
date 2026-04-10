using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Book_House.Logging;
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
    /// Логика взаимодействия для Book2.xaml
    /// </summary>
    public partial class Book2 : Page
    {
        public Book2()
        {
            InitializeComponent();
            try
            {
                var ctx = Book_houseEntities.GetContext();
                var genres = ctx.Жанры.ToList();
                var authors = ctx.Авторы.ToList();
                Жанр.ItemsSource = genres;
                Автор.ItemsSource = authors;
                AppLogger.Info($"Book2 initialized: genres={genres.Count}, authors={authors.Count}");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка инициализации Book2: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Не удалось загрузить данные для формы. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            AppLogger.Info("Переход к форме кассира из Book2");
            Manager.Forma.Navigate(new Сashier());
        }

        private void Обновить(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Жанр.Text))
                errors.AppendLine("Укажите жанр");

            if (errors.Length > 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var ctx = Book_houseEntities.GetContext();
                var genre = ctx.Жанры.FirstOrDefault(d => d.Наименование == Жанр.Text);
                if (genre == null)
                {
                    AppLogger.Info($"Жанр не найден: {Жанр.Text}");
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Жанр не найден.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var books = ctx.Книги.Where(d => d.Жанр == genre.id).ToList();
                DGridBook.ItemsSource = books;
                AppLogger.Info($"Показано книг по жанру '{genre.Наименование}': {books.Count}");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка фильтрации по жанру: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Произошла ошибка при фильтрации. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Все(object sender, RoutedEventArgs e)
        {
            try
            {
                var list = Book_houseEntities.GetContext().Книги.ToList();
                DGridBook.ItemsSource = list;
                AppLogger.Info($"Показаны все книги: {list.Count}");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка загрузки всех книг: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Не удалось загрузить книги. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                return;
            }

            try
            {
                var ctx = Book_houseEntities.GetContext();
                var av = ctx.Авторы.FirstOrDefault(d => d.Автор1 == Автор.Text);
                if (av == null)
                {
                    AppLogger.Info($"Автор не найден: {Автор.Text}");
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Автор не найден.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var books = ctx.Книги.Where(d => d.Автор == av.id_Автора).ToList();
                DGridBook.ItemsSource = books;
                AppLogger.Info($"Показано книг по автору '{av.Автор1}': {books.Count}");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка фильтрации по автору: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Произошла ошибка при фильтрации. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
