using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Book_House.Logging;

namespace Book_House
{
    /// <summary>
    /// Логика взаимодействия для Post.xaml
    /// </summary>
    public partial class Post : Page
    {
        public Post()
        {
            InitializeComponent();
            try
            {
                DGridPostav.ItemsSource = Book_houseEntities.GetContext().Поставщики.ToList();
                AppLogger.Info("Открыта страница Поставщики");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при загрузке поставщиков: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при загрузке данных. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
    }

        private void BtnEdit_click(object sender, RoutedEventArgs e)
        {
            AppLogger.Info("Переход к редактированию поставщика");
            Manager.Forma.Navigate(new PostavEdit((sender as Button).DataContext as Поставщики));
        }

        private void BtnAdd_click(object sender, RoutedEventArgs e)
        {
            AppLogger.Info("Переход к добавлению поставщика");
            Manager.Forma.Navigate(new PostavEdit(null));
        }

        private void BtnDelete_click(object sender, RoutedEventArgs e)
        {
            var PostavForRemoving = DGridPostav.SelectedItems.Cast<Поставщики>().ToList();
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
                Book_houseEntities.GetContext().Поставщики.RemoveRange(PostavForRemoving);
                Book_houseEntities.GetContext().SaveChanges();
                AppLogger.Info($"Удалено поставщиков: {PostavForRemoving.Count}");
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Данные удалены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DGridPostav.ItemsSource = Book_houseEntities.GetContext().Поставщики.ToList();
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при удалении поставщиков: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при удалении. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            AppLogger.Info("Возврат на главную страницу из Поставщики");
            Manager.Forma.Navigate(new Chief());
        }
    }
}
