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



namespace Book_House
{
    /// <summary>
    /// Логика взаимодействия для Rent2.xaml
    /// </summary>
    public partial class Rent2 : Page
    {
        public Rent2()
        {
            InitializeComponent();
        }

        private void Все(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Дата1.SelectedDate.HasValue || !Дата2.SelectedDate.HasValue)
                {
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Выберите дату начала и окончания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DateTime Date1 = Дата1.SelectedDate.Value;
                DateTime Date2 = Дата2.SelectedDate.Value;

                var ctx = Book_houseEntities.GetContext();
                var query = ctx.Продажа_книг
                    .Where(d => d.Дата_продажи > Date1 && d.Дата_продажи < Date2 && d.Статус != 2)
                    .ToList();

                DGridRent.ItemsSource = query;

                int sum = 0;
                if (query.Any())
                {
                    sum = query.Sum(c => c.К_оплате ?? 0);
                }

                Itog.Text = $"Прибыль составляет: {sum} рублей";
                Book_House.Logging.AppLogger.Info($"Сформирован отчёт по продажам: {Date1:d} - {Date2:d}, записей={query.Count}, сумма={sum}");
            }
            catch (FormatException fex)
            {
                Book_House.Logging.AppLogger.Warn("Неверный формат даты при формировании отчёта: " + fex.Message);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Нет записей за текущий период. Пожалуйста, проверьте формат даты: число.месяц.год", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                Book_House.Logging.AppLogger.Error("Ошибка при формировании отчёта: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при формировании отчёта. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var items = DGridRent.ItemsSource as IEnumerable<Продажа_книг>;
                if (items == null || !items.Any())
                {
                    Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Нет данных для экспорта.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var dlg = new Microsoft.Win32.SaveFileDialog()
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    FileName = "report.csv"
                };

                if (dlg.ShowDialog() != true)
                    return;

                var sb = new StringBuilder();
                sb.AppendLine("Название книги;Фамилия клиента;Имя клиента;Отчество клиента;Количество;Стоимость;Итого;Дата продажи");
                foreach (var it in items)
                {
                    var name = it.Книги?.Название ?? string.Empty;
                    var fam = it.Клиенты?.Фамилия ?? string.Empty;
                    var im = it.Клиенты?.Имя ?? string.Empty;
                    var otc = it.Клиенты?.Отчество ?? string.Empty;
                    var kol = it.Количество?.ToString() ?? "";
                    var price = it.Книги?.Цена.ToString() ?? "";
                    var total = it.К_оплате?.ToString() ?? "";
                    var date = it.Дата_продажи.ToString("yyyy-MM-dd");
                    sb.AppendLine($"{EscapeCsv(name)};{EscapeCsv(fam)};{EscapeCsv(im)};{EscapeCsv(otc)};{kol};{price};{total};{date}");
                }

                System.IO.File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
                Book_House.Logging.AppLogger.Info($"Экспорт продаж в CSV: {dlg.FileName}");
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Экспорт завершён.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Book_House.Logging.AppLogger.Error("Ошибка при экспорте в Excel: " + ex);
                Book_House.Controls.CustomMessageBox.Show(Window.GetWindow(this), "Ошибка при экспорте. Подробнее в логе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string EscapeCsv(string input)
        {
            if (input == null) return string.Empty;
            if (input.Contains(';') || input.Contains('"') || input.Contains('\n'))
                return '"' + input.Replace("\"", "\"\"") + '"';
            return input;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Chief());
        }
    }
}
