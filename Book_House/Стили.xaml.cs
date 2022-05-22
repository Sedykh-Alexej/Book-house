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
    /// Логика взаимодействия для Стили.xaml
    /// </summary>
    public partial class Стили : Page
    {
        public Стили()
        {
            InitializeComponent();
        }

        private void Stylee1(object sender, RoutedEventArgs e)
        {
            var Сотрудник = Book_houseEntities.GetContext().Сотрудники.Where(d => (d.id == Manager.IDSotr)).FirstOrDefault();
            Сотрудник.Стиль = @"Second_style.xaml";
            Book_houseEntities.GetContext().SaveChanges();
            var uri = new Uri(@"Second_style.xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        private void Stylee2(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(@"First_style.xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            var Сотрудник = Book_houseEntities.GetContext().Сотрудники.Where(d => (d.id == Manager.IDSotr)).FirstOrDefault();
            Сотрудник.Стиль = @"Style3.xaml";
            Book_houseEntities.GetContext().SaveChanges();
        }

        private void Stylee3(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(@"Style3.xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            var Сотрудник = Book_houseEntities.GetContext().Сотрудники.Where(d => (d.id == Manager.IDSotr)).FirstOrDefault();
            Сотрудник.Стиль = @"Style3.xaml";
            Book_houseEntities.GetContext().SaveChanges();
        }

        private void Stylee4(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(@"Style4.xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            var Сотрудник = Book_houseEntities.GetContext().Сотрудники.Where(d => (d.id == Manager.IDSotr)).FirstOrDefault();
            Сотрудник.Стиль = @"Style4.xaml";
            Book_houseEntities.GetContext().SaveChanges();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.GoBack();
        }
    }
}
