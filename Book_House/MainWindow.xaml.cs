using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Book_House
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> namelist;
        public static MainWindow window;
        public MainWindow()
        {
            InitializeComponent();
            window = this;
            Forma.Navigate(new Authorization());
            Manager.Forma = Forma;
        }

        private void Button_click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Min_But(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                MainWindow.window.DragMove();
            }
        }
    }
}
