using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ZOmicronInfo
{
    public partial class WindowLog : Window
    {
        public WindowLog()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void txtbxLog_Loaded(object sender, RoutedEventArgs e)
        {
            Log.StartWriting();
        }

        public void AppendText(string text)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => { AppendText(text); });
                return;
            }
            txtbxLog.AppendText(text);
        }
    }
}
