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
    /// Логика взаимодействия для EditPostav.xaml
    /// </summary>
    public partial class EditPostav : Page
    {
        public EditPostav()
        {
            InitializeComponent();           
            id_сотрудника.Text = Manager.IFO;
            Book.ItemsSource = Book_houseEntities.GetContext().Книги.ToList();
            поставщик.ItemsSource = Book_houseEntities.GetContext().Поставщики.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Book.Text))
                errors.AppendLine("Укажите название книги");
            if (string.IsNullOrWhiteSpace(Количество.Text))
                errors.AppendLine("Укажите количество");
            if (string.IsNullOrWhiteSpace(поставщик.Text))
                errors.AppendLine("Укажите поставщика");


           

            int Кол_во = Convert.ToInt32(Количество.Text);
            if (Кол_во <= 0)
                errors.AppendLine("Количество не может быть меньше 0");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            DateTime thisDay = DateTime.Today;
            var Книга = Book_houseEntities.GetContext().Книги.Where(d => d.Название == Book.Text).FirstOrDefault();
            Книга.Количество += Кол_во;                       
            var Поставщик = Book_houseEntities.GetContext().Поставщики.Where(d => d.Наименование == поставщик.Text).FirstOrDefault();
            Поставки поставка = new Поставки(0, Поставщик.id, Книга.id, Кол_во, thisDay, Manager.IDSotr);
            Book_houseEntities.GetContext().Поставки.Add(поставка);

            try
            {
                Book_houseEntities.GetContext().SaveChanges();
                Manager.Forma.Navigate(new Postav());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Postav());
        }
    }
}
