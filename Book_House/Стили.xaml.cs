using System;
using System.Linq;
using Book_House.Logging;

using System.Windows;
using System.Windows.Controls;

namespace Book_House
{
    /// <summary>
    /// Логика взаимодействия для Стили.xaml
    /// </summary>
    public partial class Стили : Page
    {
        public Стили()
        {
            InitializeComponent();
        }
        private void ApplyStyle(string resourcePath)
        {
            try
            {
                var uri = new Uri(resourcePath, UriKind.Relative);
                var resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                if (resourceDict == null)
                {
                    AppLogger.Error($"Не удалось загрузить ResourceDictionary: {resourcePath}");
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), $"Не удалось загрузить стиль {resourcePath}.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                AppLogger.Info($"Применён стиль: {resourcePath}");

                try
                {
                    var ctx = Book_houseEntities.GetContext();
                    var employee = ctx.Сотрудники.FirstOrDefault(d => d.id == Manager.IDSotr);
                    if (employee != null)
                    {
                        employee.Стиль = resourcePath;
                        ctx.SaveChanges();
                        AppLogger.Info($"Сохранён стиль сотрудника Id={Manager.IDSotr}: {resourcePath}");
                    }
                    else
                    {
                        AppLogger.Info($"Сотрудник с Id={Manager.IDSotr} не найден. Стиль не сохранён.");
                    }
                }
                catch (Exception ex)
                {
                    AppLogger.Error("Ошибка при сохранении стиля сотрудника: " + ex);
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка применения стиля: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Произошла ошибка при применении стиля. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Stylee1(object sender, RoutedEventArgs e)
        {
            ApplyStyle("Second_style.xaml");
        }

        private void Stylee2(object sender, RoutedEventArgs e)
        {
            ApplyStyle("First_style.xaml");
        }

        private void Stylee3(object sender, RoutedEventArgs e)
        {
            ApplyStyle("Style3.xaml");
        }

        private void Stylee4(object sender, RoutedEventArgs e)
        {
            ApplyStyle("Style4.xaml");
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.GoBack();
        }
    }
}
