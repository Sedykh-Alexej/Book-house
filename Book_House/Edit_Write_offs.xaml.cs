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
    /// Логика взаимодействия для Edit_Write_offs.xaml
    /// </summary>
    public partial class Edit_Write_offs : Page
    {
        public Edit_Write_offs()
        {
            InitializeComponent();
            id_сотрудника.Text = Manager.IFO;
            Название.ItemsSource = Book_houseEntities.GetContext().Книги.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Название.Text))
                errors.AppendLine("Укажите название книги");
            if (string.IsNullOrWhiteSpace(Количество.Text))
                errors.AppendLine("Укажите количество");
            if (string.IsNullOrWhiteSpace(Причина.Text))
                errors.AppendLine("Укажите причину");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            int Кол_во = Convert.ToInt32(Количество.Text);
            var Книга = Book_houseEntities.GetContext().Книги.Where(d => d.Название == Название.Text).FirstOrDefault();
            Книга.Количество -= Кол_во;
            Списание_книг списание = new Списание_книг(0, Книга.id, Кол_во, Причина.Text, Manager.IDSotr);
            Book_houseEntities.GetContext().Списание_книг.Add(списание);

            try
            {
                Book_houseEntities.GetContext().SaveChanges();
                Manager.Forma.Navigate(new Post());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.Navigate(new Accountant());
        }

    }
}
