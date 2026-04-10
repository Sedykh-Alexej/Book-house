using System.Windows;
using System.Windows.Media;

namespace Book_House.Controls
{
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox()
        {
            InitializeComponent();
        }

        public static MessageBoxResult Show(Window owner, string message, string title = "Уведомление системы", MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None)
        {
            var dlg = new CustomMessageBox();
            dlg.Owner = owner;
            dlg.TitleText.Text = title;
            dlg.MessageText.Text = message;

            switch (icon)
            {
                case MessageBoxImage.Warning:
                    dlg.IconImage.Source = dlg.SystemIconsToImageSource(System.Drawing.SystemIcons.Warning);
                    dlg.IconImage.Visibility = Visibility.Visible;
                    break;
                case MessageBoxImage.Error:
                    dlg.IconImage.Source = dlg.SystemIconsToImageSource(System.Drawing.SystemIcons.Error);
                    dlg.IconImage.Visibility = Visibility.Visible;
                    break;
                case MessageBoxImage.Information:
                    dlg.IconImage.Source = dlg.SystemIconsToImageSource(System.Drawing.SystemIcons.Information);
                    dlg.IconImage.Visibility = Visibility.Visible;
                    break;
                default:
                    dlg.IconImage.Visibility = Visibility.Collapsed;
                    break;
            }

            dlg.Btn1.Visibility = Visibility.Visible;
            dlg.Btn1.Content = "OK";
            dlg.Btn1.Tag = MessageBoxResult.OK;
            dlg.Btn2.Visibility = Visibility.Collapsed;
            dlg.Btn3.Visibility = Visibility.Collapsed;

            if (buttons == MessageBoxButton.OKCancel)
            {
                dlg.Btn1.Content = "OK";
                dlg.Btn1.Tag = MessageBoxResult.OK;
                dlg.Btn2.Visibility = Visibility.Visible;
                dlg.Btn2.Content = "Отмена";
                dlg.Btn2.Tag = MessageBoxResult.Cancel;
            }
            else if (buttons == MessageBoxButton.YesNo)
            {
                dlg.Btn1.Content = "Да";
                dlg.Btn1.Tag = MessageBoxResult.Yes;
                dlg.Btn2.Visibility = Visibility.Visible;
                dlg.Btn2.Content = "Нет";
                dlg.Btn2.Tag = MessageBoxResult.No;
            }
            else if (buttons == MessageBoxButton.YesNoCancel)
            {
                dlg.Btn1.Content = "Да";
                dlg.Btn1.Tag = MessageBoxResult.Yes;
                dlg.Btn2.Visibility = Visibility.Visible;
                dlg.Btn2.Content = "Нет";
                dlg.Btn2.Tag = MessageBoxResult.No;
                dlg.Btn3.Visibility = Visibility.Visible;
                dlg.Btn3.Content = "Отмена";
                dlg.Btn3.Tag = MessageBoxResult.Cancel;
            }

            dlg.ShowDialog();
            return dlg._result;
        }

        private MessageBoxResult _result = MessageBoxResult.None;

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is MessageBoxResult res)
            {
                _result = res;
                this.DialogResult = true;
            }
            else
            {
                _result = MessageBoxResult.None;
                this.DialogResult = false;
            }
            Close();
        }

        private ImageSource SystemIconsToImageSource(System.Drawing.Icon icon)
        {
            if (icon == null) return null;
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
