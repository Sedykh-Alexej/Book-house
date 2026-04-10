using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Book_House.Logging;
using Book_House.Security;

namespace Book_House
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        private List<string> _nameList;

        public Authorization()
        {
            InitializeComponent();

            _nameList = new List<string>();

            try { if (BtnLogin != null) BtnLogin.IsEnabled = false; } catch { }

            try { if (outText != null) outText.TextChanged += OutText_TextChanged; } catch { }

            try
            {
                using (var ctx = new Book_houseEntities())
                {
                    _nameList = ctx.Сотрудники
                        .Select(s => s.Фамилия + " " + s.Имя + " " + s.Отчество)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("Не удалось загрузить список сотрудников: " + ex);
                _nameList = new List<string>();
                try { FeedbackText.Text = "Не удалось загрузить список сотрудников."; } catch { }
            }
        }

        private void OutText_TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                BtnLogin.IsEnabled = !string.IsNullOrEmpty(PasswordBoxx.Password) && !string.IsNullOrEmpty(outText.Text);
                FeedbackText.Text = string.Empty;
            }
            catch { }
        }

        private void Populating(object sender, PopulatingEventArgs e)
        {
            try
            {
                var txt = outText.Text ?? string.Empty;
                var list = string.IsNullOrWhiteSpace(txt)
                    ? new List<string>()
                    : _nameList.Where(x => x.StartsWith(txt, StringComparison.OrdinalIgnoreCase)).ToList();

                outText.ItemsSource = list;
                outText.PopulateComplete();
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка автодополнения: " + ex);
                try { FeedbackText.Text = "Ошибка автодополнения."; } catch { }
            }
        }

        private void Войти(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = (outText.Text ?? string.Empty).Trim();
                string providedPassword = PasswordBoxx.Password ?? string.Empty;

                using (var ctx = new Book_houseEntities())
                {
                    var emp = ctx.Сотрудники
                        .FirstOrDefault(d => (d.Фамилия + " " + d.Имя + " " + d.Отчество).Equals(login, StringComparison.OrdinalIgnoreCase));

                    if (emp == null)
                    {
                        AppLogger.Info("Неудачная попытка входа (пользователь не найден): " + login);
                        try { FeedbackText.Text = "Неверное имя пользователя или пароль."; } catch { }
                        return;
                    }

                    if (!PasswordHelper.VerifyPassword(emp.Пароль, providedPassword))
                    {
                        AppLogger.Info("Неудачная попытка входа (неверный пароль): " + login);
                        try { FeedbackText.Text = "Неверное имя пользователя или пароль."; } catch { }
                        return;
                    }

                    Manager.IDSotr = emp.id;
                    Manager.IFO = emp.Фамилия + " " + emp.Имя + " " + emp.Отчество;

                    if (!string.IsNullOrWhiteSpace(emp.Стиль))
                    {
                        try
                        {
                            var uri = new Uri(emp.Стиль, UriKind.Relative);
                            var dict = Application.LoadComponent(uri) as ResourceDictionary;
                            if (dict != null)
                            {
                                Application.Current.Resources.Clear();
                                Application.Current.Resources.MergedDictionaries.Add(dict);
                            }
                        }
                        catch (Exception ex)
                        {
                            AppLogger.Error("Ошибка загрузки стиля: " + ex);
                        }
                    }

                    if (emp.Должность == 1)
                        Manager.Forma.Navigate(new Сashier());
                    else if (emp.Должность == 2)
                        Manager.Forma.Navigate(new Chief());
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("Ошибка входа: " + ex);
                try { FeedbackText.Text = "Ошибка при входе. Обратитесь к администратору."; } catch { }
            }
        }

        private void PasswordBoxx_PasswordChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                BtnLogin.IsEnabled = !string.IsNullOrEmpty(PasswordBoxx.Password) && !string.IsNullOrEmpty(outText.Text);
                FeedbackText.Text = string.Empty;
            }
            catch { }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
