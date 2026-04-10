using Book_House.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для BookEdit.xaml
    /// </summary>
    public partial class BookEdit : Page
    {
        public Книги _currentКниги = new Книги();
        public List<string> namelist;
        public Book_houseEntities db;
        public BookEdit(Книги selectedКниги)
        {
            InitializeComponent();
            if (selectedКниги != null)
                _currentКниги = selectedКниги;

            DataContext = _currentКниги;
            try
            {
                Жанр.ItemsSource = Book_houseEntities.GetContext().Жанры.ToList();
                db = new Book_houseEntities();
                namelist = new List<string>();
                foreach (var item in db.Авторы)
                {
                    namelist.Add(item.Автор1);
                }
                AppLogger.Info($"Открыта страница редактирования книги Id={_currentКниги.id}");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при инициализации BookEdit: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при загрузке данных. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Populating(object sender, PopulatingEventArgs e)
        {
            try
            {
                string txt = outAvtor.Text;
                List<string> outoList = new List<string>();
                outoList.Clear();
                if (namelist != null)
                {
                    foreach (string item in namelist)
                    {
                        if (!string.IsNullOrEmpty(outAvtor.Text))
                        {
                            if (item.ToLower().StartsWith(txt.ToLower()))
                            {
                                outoList.Add(item);
                            }
                        }
                    }
                    outAvtor.ItemsSource = outoList;
                    outAvtor.PopulateComplete();
                }
            }
            catch (Exception er)
            {
                AppLogger.Error("Ошибка при автоподборе автора: " + er);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка автодополнения. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentКниги.Название))
                errors.AppendLine("Укажите название");
            if (string.IsNullOrWhiteSpace(Цена.Text))
                errors.AppendLine("Цена не заполнена");
            if (string.IsNullOrWhiteSpace(Количество.Text))
                errors.AppendLine("Количество не заполнено");
            if (_currentКниги.Цена <= 0)
                errors.AppendLine("Укажите корректную цену");
            if (_currentКниги.Количество <= 0)
                errors.AppendLine("Количество не может быть меньше или равно 0");
            if (string.IsNullOrWhiteSpace(Жанр.Text))
                errors.AppendLine("Укажите жанр");
            if (string.IsNullOrWhiteSpace(outAvtor.Text))
                errors.AppendLine("Укажите автора");

            if (errors.Length > 0)
            {
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                AppLogger.Warn($"Валидация книги не пройдена: {errors}");
                return;
            }

            try
            {
                var ctx = Book_houseEntities.GetContext();
                if (_currentКниги.id == 0)
                {
                    ctx.Книги.Add(_currentКниги);
                    AppLogger.Info("Добавлена новая книга в контекст");
                }

                var Avtor = ctx.Авторы.FirstOrDefault(d => d.Автор1 == outAvtor.Text);
                if (Avtor == null)
                {
                    Авторы Avtorr = new Авторы(0, outAvtor.Text);
                    ctx.Авторы.Add(Avtorr);
                    ctx.SaveChanges();
                    _currentКниги.Автор = Avtorr.id_Автора;
                    AppLogger.Info($"Добавлен новый автор: {outAvtor.Text}");
                }
                else
                {
                    _currentКниги.Автор = Avtor.id_Автора;
                }

                try
                {
                    ctx.SaveChanges();
                    AppLogger.Info($"Книга сохранена Id={_currentКниги.id}");
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Данные книги сохранены", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    Manager.Forma.Navigate(new Books1());
                }
                catch (Exception ex)
                {
                    AppLogger.Error("Ошибка при сохранении книги: " + ex);
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при сохранении книги. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка при подготовке данных для сохранения книги: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при обработке данных. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            try
            {
                var ctx = Book_houseEntities.GetContext();
                var entry = ctx.Entry(_currentКниги);
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
                AppLogger.Error("Ошибка при отмене изменений книг: " + ex);
            }

            Manager.Forma.Navigate(new Books1());
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
