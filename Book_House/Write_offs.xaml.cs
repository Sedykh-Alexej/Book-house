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
    /// Логика взаимодействия для Write_offs.xaml
    /// </summary>
    public partial class Write_offs : Page
    {
        public Write_offs()
        {
            InitializeComponent();
            try
            {
                Book.ItemsSource = Book_houseEntities.GetContext().Книги.ToList();
                AppLogger.Info("Открыта страница Списания");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при загрузке данных на странице списания: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при загрузке данных. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void BtnAdd_click(object sender, RoutedEventArgs e)
        {
            AppLogger.Info("Переход на страницу добавления списания");
            Manager.Forma.Navigate(new Edit_Write_offs());
        }



        private void BtnDelete_click(object sender, RoutedEventArgs e)
        {
            var PostForRemoving = DGridPost.SelectedItems.Cast<Списание_книг>().ToList();
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
                Book_houseEntities.GetContext().Списание_книг.RemoveRange(PostForRemoving);
                Book_houseEntities.GetContext().SaveChanges();
                AppLogger.Info($"Удалено записей списания: {PostForRemoving.Count}");
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Данные удалены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DGridPost.ItemsSource = Book_houseEntities.GetContext().Списание_книг.ToList();
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при удалении списаний: " + ex);
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
                DGridPost.ItemsSource = Book_houseEntities.GetContext().Списание_книг.ToList();
                AppLogger.Info("Загружены все списания");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при загрузке списаний: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при загрузке данных. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Обновить(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Book.Text))
                errors.AppendLine("Укажите книгу");

            if (errors.Length > 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                AppLogger.Warn($"Валидация фильтра книги для списания не пройдена: {errors}");
                return;
            }

            try
            {
                var ctx = Book_houseEntities.GetContext();
                var Книга = ctx.Книги.FirstOrDefault(d => d.Название == Book.Text);
                if (Книга == null)
                {
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Книга не найдена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    AppLogger.Warn($"Книга для фильтра списания не найдена: {Book.Text}");
                    return;
                }
                DGridPost.ItemsSource = ctx.Списание_книг.Where(d => d.id_Книги == Книга.id).ToList();
                AppLogger.Info($"Фильтр списания по книге: {Книга.Название}");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при фильтрации списаний: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при фильтрации. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
