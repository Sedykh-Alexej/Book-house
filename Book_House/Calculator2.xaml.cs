using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для Calculator2.xaml
    /// </summary>
    public partial class Calculator2 : Page
    {
        public Calculator2()
        {
            InitializeComponent();
            foreach (UIElement el in Calculatorr.Children)
            {
                if (el is Button)
                {
                    ((Button)el).Click += Button_Click;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string str = (string)((Button)e.OriginalSource).Content;
            if (str == "AC")
                TXT.Text = "";
            else if (str == "=")
            {
                string value = new DataTable().Compute(TXT.Text, null).ToString();
                TXT.Text = value;
            }
            else
                TXT.Text += str;

        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Manager.Forma.GoBack();
        }
    }
}
