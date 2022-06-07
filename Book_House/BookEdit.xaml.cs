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
            Жанр.ItemsSource = Book_houseEntities.GetContext().Жанры.ToList();
            db = new Book_houseEntities();
            namelist = new List<string>();
            foreach (var item in db.Авторы)
            {
                namelist.Add(item.Автор1);
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
                MessageBox.Show(er.ToString());
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

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
                MessageBox.Show(errors.ToString());
                return;
            }


            if (_currentКниги.id == 0)
            {
                Book_houseEntities.GetContext().Книги.Add(_currentКниги);
            }

            try
            {
                var Avtor = Book_houseEntities.GetContext().Авторы.Where(d => d.Автор1 == outAvtor.Text).FirstOrDefault();
                if (Avtor == null)
                {
                    Авторы Avtorr = new Авторы(0, outAvtor.Text);
                    Book_houseEntities.GetContext().Авторы.Add(Avtorr);
                    _currentКниги.Автор = Avtorr.id_Автора;
                }
                else
                {
                    _currentКниги.Автор = Avtor.id_Автора;
                }
                Book_houseEntities.GetContext().SaveChanges();
                Manager.Forma.Navigate(new Books1());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
